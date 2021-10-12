using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityDLL;
using STLExtensiton;
using DG.Tweening;

[DisallowMultipleComponent]
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    #region  Variables 
    public class BGM
    {
        public const string PATH = "Audio/BGM";
        public const string VOLUME_KEY = "BGM_VOLUME_KEY";

        public class DEFAULT
        {
            public const float VOLUME = 1.0f;
        }
    }
    public class SE
    {
        public const string PATH = "Audio/SE";
        public const string VOLUME_KEY = "SE_VOLUME_KEY";
        public class DEFAULT
        {
            public const float VOLUME = 1.0f;
        }
    }
    // Time to take though BGM performs fading.
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private static float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    // Audio name to flow next
    private static string _nextBGMName;
    private static string _nextSEName;

    private static bool _isFadeOut = false;

    private static AudioSource _bgmSource;
    private static List<AudioSource> _seSourceList;
    private const int SE_SOURCE_NUM = 10;

    //public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    //public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.


    private static Dictionary<string, AudioClip> _bgmDic;
    private static Dictionary<string, AudioClip> _seDic;
    #endregion

    private new void Awake()
    {
        base.Awake();

        #region Add components
        // Making Audiolistener and AudioSource.
        if (!Camera.main?.GetComponent<AudioListener>())
            gameObject.AddComponent<AudioListener>();


        for (int i = 0; i < SE_SOURCE_NUM + 1; i++)
            gameObject.AddComponent<AudioSource>();

        #endregion

        // Getting audio source and setting to each variable, then setting that volume
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        _seSourceList = new List<AudioSource>();

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i].playOnAwake = false;

            if (i == 0)
            {
                audioSourceArray[i].loop = true;
                _bgmSource = audioSourceArray[i];
                _bgmSource.volume = PlayerPrefs.GetFloat(BGM.VOLUME_KEY, BGM.DEFAULT.VOLUME);
            }
            else
            {
                _seSourceList.Add(audioSourceArray[i]);
                audioSourceArray[i].volume = PlayerPrefs.GetFloat(SE.VOLUME_KEY, SE.DEFAULT.VOLUME);
            }
        }

        //Setting and loading all SE and BGM files from resources folder.
        _bgmDic = new Dictionary<string, AudioClip>();
        _seDic = new Dictionary<string, AudioClip>();
        // print(Resources.LoadAll(BGM.PATH) as AudioClip[]);
        object[] bgmList = Resources.LoadAll(BGM.PATH);
        object[] seList = Resources.LoadAll(SE.PATH);

        foreach (AudioClip bgm in bgmList)
        {
            _bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            _seDic[se.name] = se;
        }
    }

    private void Update()
    {
        if (!_isFadeOut)
            return;

        // Its volume downs gradually and its volume restores defaults if it becomes 0 then flow next bgm.

        _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (_bgmSource.volume <= 0)
        {
            _bgmSource.Stop();
            _bgmSource.volume = PlayerPrefs.GetFloat(BGM.VOLUME_KEY, BGM.DEFAULT.VOLUME);
            _isFadeOut = false;

            if (!string.IsNullOrEmpty(_nextBGMName))
                PlayBGM(_nextBGMName);
        }
    }

    #region For SE

    public static void PlaySE(string seName, float delaySecond = 0f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.LogWarning(seName + "is nothing.");
            return;
        }

        _nextSEName = seName;
        DOVirtual.DelayedCall(delaySecond, PlaySE);
    }

    private static void PlaySE()
    {
        foreach (AudioSource seSources in _seSourceList)
        {
            if (!seSources.isPlaying)
            {
                seSources.PlayOneShot(_seDic[_nextSEName]);
                return;
            }
        }
    }

    #endregion

    #region For BGM

    public static void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!_bgmDic.ContainsKey(bgmName))
        {
            Debug.LogWarning(bgmName + " is nothing.");
            return;
        }

        //  It flows when bgm doesn't flow.
        if (!_bgmSource.isPlaying)
        {
            _nextBGMName = "";
            _bgmSource.clip = _bgmDic.TryGetValueEx(bgmName, null);
            _bgmSource.Play();
        }

        // if any bgm flows already, flow next bgm after it fade out.(If next bgm and currently bgm are same, it is through.)
        else if (_bgmSource.clip.name != bgmName)
        {
            _nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    public static void StopBGM()
    {
        _bgmSource.Stop();
    }

    public static void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        _bgmFadeSpeedRate = fadeSpeedRate;
        _isFadeOut = true;
    }

    #endregion

    #region For volume

    public static void ChangeBothVolume(float BGMVolume, float SEVolume)
    {
        _bgmSource.volume = BGMVolume;
        foreach (AudioSource seSources in _seSourceList)
        {
            seSources.volume = SEVolume;
        }

    }

    public static void ChangeBGMVolume(float BGMVolume)
    {
        _bgmSource.volume = BGMVolume;
    }

    public static void ChangeSEVolume(float SEVolume)
    {
        foreach (AudioSource seSources in _seSourceList)
        {
            seSources.volume = SEVolume;
        }
    }


    #endregion

    public IEnumerator FourOnTheFloor(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
        }
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        //int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        //float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        //_seSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        //_seSource.clip = clips[randomIndex];

        //Play the clip.
        //_seSource.Play();
    }
}
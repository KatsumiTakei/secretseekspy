
using UnityEngine;
using DG.Tweening;
using UnityDLL;
using System.Collections;

public class Securitygurad : MonoBehaviour
    , IGunTarget
    , IInitializable
{
    public class Anim
    {
        SpriteRenderer spriteRenderer = null;
        Sprite[] anims = null;

        int animCnt = 0;
        int animFreamCnt = 0;
        int dirIndex = 0;
        const int AnimChangeDelimitationIndex = 4;


        public Anim(SpriteRenderer spriteRenderer, Sprite[] anims)
        {
            this.spriteRenderer = spriteRenderer;
            this.anims = anims;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveDir"> 0:down 1:up 2:left 3:right </param>
        public void MoveAnim(int moveDir)
        {
            dirIndex = AnimChangeDelimitationIndex * moveDir;
            spriteRenderer.sprite = anims[dirIndex + (animCnt % 2 + 1)];
            animFreamCnt++;
            if (animFreamCnt % 4 == 1)
                animCnt++;
        }

        public void StopAnim()
        {
            spriteRenderer.sprite = anims[dirIndex];
            spriteRenderer.SetAlpha(1f);
        }

        public void SleepAnim()
        {
            int adjust = animCnt % 4;
            spriteRenderer.SetAlpha(Mathf.Clamp(Mathf.Sin(animCnt), 0.4f, 0.8f));

            spriteRenderer.sprite = anims[3 * (adjust + 1) + adjust];
            animFreamCnt++;
            if (animFreamCnt % 4 == 1)
                animCnt++;
        }

    }

    public class Model
    {
        public Anim anim = null;
        public Waypoint waypoint = null;
        public SearchingBehavior searchingBehavior = null;
        public Transform transform = null;

        public Model(Transform transform, Waypoint waypoint,  SearchingBehavior searchingBehavior, Anim anim)
        {
            this.anim = anim;
            this.waypoint = waypoint;
            this.searchingBehavior = searchingBehavior;
            this.transform = transform;
        }
    }


    [SerializeField]
    Sprite[] anims = null;

    [SerializeField]
    Waypoint waypoint = null;

    const int ActionInterval = 4;
    int actionCnt = ActionInterval;

    SecurityStateMachine stateMachine = new SecurityStateMachine();
    Model model = null;

    Vector3 initPos = Vector3.zero;

    public void Hit()
    {
        stateMachine.ChangeState(SecurityStateMachine.Type.Sleep, model);
    }

    public void Initialize()
    {
        transform.position = initPos;
        waypoint.SearchNearPoint(transform.position);
        actionCnt = ActionInterval;
    }

    IEnumerator CoLateStart()
    {
        yield return null;
        waypoint.SearchNearPoint(transform.position);
    }

    void Awake()
    {
        model = new Model(transform, waypoint, GetComponentInChildren<SearchingBehavior>(), new Anim(GetComponent<SpriteRenderer>(), anims));
    }

    void Start()
    {
        initPos = transform.position;
        StartCoroutine(CoLateStart());
    }

    void Update()
    {
        if (ActionInterval <= actionCnt)
        {
            stateMachine.Execute(model);
        }
        else if (actionCnt <= 0)
        {
            actionCnt = ActionInterval;
            return;
        }

        actionCnt--;

    }

}

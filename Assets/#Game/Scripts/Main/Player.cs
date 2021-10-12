using UnityEngine;
using DG.Tweening;
using System.Collections;

public static class MoveDirection
{
    public const int Down = 0;
    public const int Up = 1;
    public const int Left = 2;
    public const int Right = 3;
}


public class Player : MonoBehaviour
    , IInitializable
{
    class ModelPlayerBullet
    {
        public int currentBulletNum = BulletMin;
        public const int BulletMax = 10;
        public const int BulletMin = 0;
        public const int InitBulletNum = 3;
    }

    #region		Variables 
    [SerializeField]
    private LayerMask layerMask = default;

    [SerializeField]
    Sprite[] anims = null;

    Rigidbody2D rigidbody2d = null;
    SpriteRenderer spriteRenderer = null;
    PlayerAnim playerAnim = null;
    Transform refTransform = null;
    Collider2D collider2D = null;
    ModelPlayerBullet modelPlayerBullet = new ModelPlayerBullet();

    IWeapon currentWeapon = null;

    const int ActionInterval = 4;
    int actionCnt = 0;

    int currentDirction = 0;
    int moveDirection = -1;

    Vector3 initPos = Vector3.zero;

    Transform targetObj = null;

    const float Spd = TileMapManager.TileSize * 0.5f;
    readonly Vector2[] MoveValues = {
        Vector2.down * Spd,
        Vector2.up* Spd,
        Vector2.left* Spd,
        Vector2.right * Spd
    };

    #endregion  Variables 

    #region		Functions 

    void Move(int moveIndex)
    {
        rigidbody2d.MovePosition(rigidbody2d.position + MoveValues[moveIndex]);
        playerAnim.MoveAnim(moveIndex);
    }

    void Survey()
    {
        if (targetObj)
        {// Surveyed
            targetObj.GetComponent<ISurveyTarget>().Surveyed(this);
            EventManager.BroadcastTargetingObj(false);
        }

        targetObj = null;
    }

    void Search()
    {
        //  下向きの場合のみ探索距離に補正
        int adjust = (currentDirction == MoveDirection.Down) ? 3 : 1;

        var frontTarget = Physics2D.Linecast(
            refTransform.position
            , new Vector2(refTransform.position.x, refTransform.position.y) + MoveValues[currentDirction] * adjust
            , layerMask).transform;

        var isTarget = (frontTarget == null) ? false : frontTarget.CompareTag("SurveyTarget");

        if (isTarget)
        {
            targetObj = frontTarget;
        }
        else
        {
            targetObj = null;
        }

        EventManager.BroadcastTargetingObj(isTarget);
    }

    void Shot()
    {
        if (ModelPlayerBullet.BulletMin >= modelPlayerBullet.currentBulletNum)
            return;

        AudioManager.PlaySE("Shot");
        playerAnim.UseWeaponAnim();

        currentWeapon.Use(MoveValues[currentDirction]);

        modelPlayerBullet.currentBulletNum--;
        EventManager.BroadcastShot(modelPlayerBullet.currentBulletNum);
    }

    void GetBullet()
    {
        if (ModelPlayerBullet.BulletMax <= modelPlayerBullet.currentBulletNum)
            return;

        EventManager.BroadcastGetBullet(modelPlayerBullet.currentBulletNum);
        modelPlayerBullet.currentBulletNum++;
    }


    public void MoveRoom()
    {
        int moveIndex = currentDirction;
        collider2D.enabled = false;
        transform
            .DOLocalMove(new Vector3(MoveValues[moveIndex].x, MoveValues[moveIndex].y) * 4, 0.1f)
            .SetRelative()
            .OnComplete(() => collider2D.enabled = true);
        playerAnim.MoveAnim(moveIndex);

    }

    public void Initialize()
    {
        targetObj = null;
        transform.position = initPos;
        playerAnim.MoveAnim(0);
        currentDirction = 0;
        moveDirection = -1;

        modelPlayerBullet.currentBulletNum = 0;
        StartCoroutine(CoGetBullet(ModelPlayerBullet.InitBulletNum));
    }

    public IEnumerator CoGetBullet(int getBulletNum)
    {
        int max = modelPlayerBullet.currentBulletNum + getBulletNum;
        for (int i = modelPlayerBullet.currentBulletNum; i < max; i++)
        {
            yield return null;
            GetBullet();
        }
    }

    #endregion  Functions 

    #region		Events

    void OnMultipleInput(eInputType inputType)
    {
        if (actionCnt > 0)
            return;

        actionCnt = ActionInterval;

        switch (inputType)
        {
            case eInputType.MoveDownKey:
            case eInputType.MoveUpKey:
            case eInputType.MoveLeftKey:
            case eInputType.MoveRightKey:

                moveDirection = (int)inputType % 4;
                Search();
                break;

            case eInputType.ShotAndDecideKeyDown:

                Shot();
                break;

            case eInputType.SurveyAndCanselKeyDown:

                Survey();
                break;

        }
    }

    void OnEnable()
    {
        EventManager.OnMultipleInput += OnMultipleInput;
    }
    void OnDisable()
    {
        EventManager.OnMultipleInput -= OnMultipleInput;
    }


    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentWeapon = GetComponentInChildren<Gun>();
        playerAnim = new PlayerAnim(spriteRenderer, anims);
        refTransform = transform;

        initPos = refTransform.position;
        Initialize();
    }

    void FixedUpdate()
    {
        if (ActionInterval <= actionCnt)
        {
            if (moveDirection >= 0)
            {
                Move(moveDirection);
                currentDirction = moveDirection;
                moveDirection = -1;
            }
        }
        else if (actionCnt <= 0)
        {
            playerAnim.StopAnim();
            moveDirection = -1;
        }

        actionCnt--;
    }

    #endregion  Events 
}
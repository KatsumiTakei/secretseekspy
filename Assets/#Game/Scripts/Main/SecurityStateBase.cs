
using UnityDLL;
using UnityEngine;

public class SecurityStateMachine
{
    public class Type
    {
        public const int Default = 0;
        public const int Wait = 1;
        public const int Sleep = 2;
    }

    SecurityStateBase currentState = null;
    SecurityStateBase[] stateArray = {
        new SecurityStateMove(),
        new SecurityStateWait(),
        new SecurityStateSleep(),
    };

    public SecurityStateMachine()
    {
        currentState = stateArray[0];
    }

    public void Execute(Securitygurad.Model model)
    {
        currentState.Execute(this, model);
    }

    public void ChangeState(int securityState, Securitygurad.Model model)
    {
        currentState = stateArray[securityState];
        currentState.OnEnable(model);
    }
}

public abstract class SecurityStateBase
{
    public abstract void Execute(SecurityStateMachine stateMachine, Securitygurad.Model model);
    public abstract void OnEnable(Securitygurad.Model model);
}

public class SecurityStateWait : SecurityStateBase
{
    int stepIntervalCount = 0;
    const int StepInterval = 15;

    public override void Execute(SecurityStateMachine stateMachine, Securitygurad.Model model)
    {
        stepIntervalCount++;

        if (StepInterval < stepIntervalCount)
        {
            model.waypoint.SetNextDestination();
            stateMachine.ChangeState(SecurityStateMachine.Type.Default, model);
        }
    }

    public override void OnEnable(Securitygurad.Model model)
    {
        stepIntervalCount = 0;
        model.anim.StopAnim();
    }
}

public class SecurityStateMove : SecurityStateBase
{
    readonly float[] SearchDirction = { 180f, 0f, 90f, 270f };

    const float Spd = TileMapManager.TileSize * 0.4f;
    readonly Vector2[] Direction = {
        Vector2.down * Spd,
        Vector2.up* Spd,
        Vector2.left* Spd,
        Vector2.right * Spd
    };

    void Move(Transform transform, Securitygurad.Anim anim, SearchingBehavior searchingBehavior, int moveIndex)
    {
        transform.AddPositionXY(Direction[moveIndex]);
        anim.MoveAnim(moveIndex);
        searchingBehavior.transform.localEulerAngles = new Vector3(0f, 0f, SearchDirction[moveIndex]);
    }

    public override void Execute(SecurityStateMachine stateMachine, Securitygurad.Model model)
    {
        int moveDirection = model.waypoint.CalcStepDirection(model.transform.position);
        if (moveDirection < 0)
        {
            stateMachine.ChangeState(SecurityStateMachine.Type.Wait, model);
            return;
        }

        Move(model.transform, model.anim, model.searchingBehavior, moveDirection);
    }

    public override void OnEnable(Securitygurad.Model model)
    {
        model.searchingBehavior.gameObject.SetActive(true);
    }
}

public class SecurityStateSleep : SecurityStateBase
{
    const int SleepTime = 180;
    int sleepCount = 0;

    public override void Execute(SecurityStateMachine stateMachine, Securitygurad.Model model)
    {
        model.anim.SleepAnim();
        sleepCount++;

        if (sleepCount > SleepTime)
        {
            stateMachine.ChangeState(SecurityStateMachine.Type.Default, model);
        }
    }

    public override void OnEnable(Securitygurad.Model model)
    {
        sleepCount = 0;
        model.searchingBehavior.gameObject.SetActive(false);
        AudioManager.PlaySE("Hit");
    }
}


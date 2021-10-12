
public partial class InputManager
{


    public static void ManualUpdate()
    {
        Keyboard.Update();
        
    }
}

public enum eInputType
{
    MoveUpKeyDown,
    MoveUpKey,
    MoveUpKeyUp,
    MoveDownKeyDown,
    MoveDownKey,
    MoveDownKeyUp,
    MoveRightKeyDown,
    MoveRightKey,
    MoveRightKeyUp,
    MoveLeftKeyDown,
    MoveLeftKey,
    MoveLeftKeyUp,

    MoveRuKey,
    MoveRdKey,
    MoveLuKey,
    MoveLdKey,


    ShotAndDecideKeyDown,
    SurveyAndCanselKeyDown,


}

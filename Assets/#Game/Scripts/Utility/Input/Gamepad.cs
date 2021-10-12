//using System.Linq;
//using System.Collections.Generic;
//using UnityEngine;
//using STLExtensiton;
//using static GamepadInput.GamePad;

//public partial class InputManager
//{
//    public partial class Gamepad
//    {
     
//        #region variable
//        PadInputModule shotAndDecide = new PadInputModule(Button.A);
//        PadInputModule bombAndCansel = new PadInputModule(Button.B);

//        PadInputModule pause = new PadInputModule(Trigger.RightTrigger);
//        PadInputModule speedChange = new PadInputModule(Trigger.LeftTrigger);

//        List<PadInputModule> usedInputs = new List<PadInputModule>();
//        Index controlIndex;

//        #endregion variable


//        #region private
//        #region GetButton

//        public static void GetButtonDown(eInputType inputType, Button button, Index controlIndex)
//        {
//            if (GamepadInput.GamePad.GetButtonDown(button, controlIndex))
//                EventManager.BroadcastMultipleInput(inputType);
//        }
//        public static void GetButtonUp(eInputType inputType, Button button, Index controlIndex)
//        {
//            if (GamepadInput.GamePad.GetButtonUp(button, controlIndex))
//                EventManager.BroadcastMultipleInput(inputType);
//        }
//        public static void GetButton(eInputType inputType, Button button, Index controlIndex)
//        {
//            if (GamepadInput.GamePad.GetButton(button, controlIndex))
//                EventManager.BroadcastMultipleInput(inputType);
//        }
//        #endregion GetButton

//        #region GetAxis

//        public static void GetAxis(Axis axis, Index controlIndex, bool raw = false)
//        {
//            var value = GamepadInput.GamePad.GetAxis(axis, controlIndex, raw);
//            eInputType? eInput = null;
//            if (value.x > 0)
//                eInput = eInputType.MoveRightKey;
//            if (value.x < 0)
//                eInput = eInputType.MoveLeftKey;

//            if (value.y > 0)
//            {
//                if (eInput == eInputType.MoveRightKey)
//                    eInput = eInputType.MoveRuKey;
//                else if (eInput == eInputType.MoveLeftKey)
//                    eInput = eInputType.MoveLuKey;
//                else
//                    eInput = eInputType.MoveUpKey;

//            }
//            if (value.y < 0)
//            {
//                if (eInput == eInputType.MoveRightKey)
//                    eInput = eInputType.MoveRdKey;
//                else if (eInput == eInputType.MoveLeftKey)
//                    eInput = eInputType.MoveLdKey;
//                else
//                    eInput = eInputType.MoveDownKey;
//            }

//            EventManager.BroadcastMultipleInput(eInput.Value);
//        }
//        #endregion GetAxis

//        #region GetTrigger
//        public static void GetTrigger(eInputType inputType, Trigger trigger, Index controlIndex, bool onCurrentTrigger, bool previewTrigger)
//        {
//            if (onCurrentTrigger)
//                EventManager.BroadcastMultipleInput(inputType);

//        }
//        public static void GetTriggerDown(eInputType inputType, Trigger trigger, Index controlIndex, bool onCurrentTrigger, bool previewTrigger)
//        {
//            if (onCurrentTrigger && !previewTrigger)
//                EventManager.BroadcastMultipleInput(inputType);
//        }

//        public static void GetTriggerUp(eInputType inputType, Trigger trigger, Index controlIndex, bool onCurrentTrigger, bool previewTrigger)
//        {
//            if (!onCurrentTrigger && previewTrigger)
//                EventManager.BroadcastMultipleInput(inputType);
//        }


//        #endregion GetTrigger
//        #endregion private

//        #region public
//        public Gamepad(Index controlIndex)
//        {
//            this.controlIndex = controlIndex;

//            usedInputs.Add(shotAndDecide);
//            shotAndDecide.SetEvent(eInputType.ShotAndDecideKey, GetButton, GetTrigger);
//            shotAndDecide.SetEvent(eInputType.ShotAndDecideKeyDown, GetButtonDown, GetTriggerDown);
//            shotAndDecide.SetEvent(eInputType.ShotAndDecideKeyUp, GetButtonUp, GetTriggerUp);

//            usedInputs.Add(bombAndCansel);
//            bombAndCansel.SetEvent(eInputType.BombAndCanselKeyDown, GetButtonDown, GetTriggerDown);

//            usedInputs.Add(pause);
//            pause.SetEvent(eInputType.PauseKeyDown, GetButtonDown, GetTriggerDown);

//            usedInputs.Add(speedChange);
//            speedChange.SetEvent(eInputType.SpeedChangeKeyDown, GetButtonDown, GetTriggerDown);
//            speedChange.SetEvent(eInputType.SpeedChangeKeyUp, GetButtonUp, GetTriggerUp);
//        }

//        public void ConfigPadKey()
//        {// TODO    :
//        }

//        public bool IsRegisteredButton(Button button)
//        {
//            return usedInputs.Any(p => p.GetButton() == button);
//        }
//        public bool IsRegisteredTrigger(Trigger trigger)
//        {
//            return usedInputs.Any(p => p.GetTrigger() == trigger);
//        }

//        public void Update()
//        {
//            GetAxis(Axis.LeftStick, controlIndex);
//            usedInputs.ForEach(p => p.Update(controlIndex));
//        }

//        #endregion public

//        public static int GetConnectNum()
//        {
//            int connectNum = 0;
//            var pads = Input.GetJoystickNames();
//            pads.ForEach(p =>
//            {
//                if (!string.IsNullOrEmpty(p))
//                    connectNum++;
//            });
//            return connectNum;
//        }
//    }
//}

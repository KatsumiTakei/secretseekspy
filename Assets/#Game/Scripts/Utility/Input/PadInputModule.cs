//using System;
//using static GamepadInput.GamePad;

//public partial class InputManager
//{
//    public partial class Gamepad
//    {
//        public class PadInputModule
//        {
//            Button? Button { get; set; } = null;
//            Trigger? Trigger { get; set; } = null;
//            bool isPreviewTriggerPressed = false;
//            bool isCurrentTriggerPressed = false;

//            event Action<Button, Index> ButtonInput;
//            event Action<Trigger, Index, bool, bool> TriggerInput;

//            public PadInputModule(Button button) { SetButton(button); }
//            public PadInputModule(Trigger trigger) { SetTrigger(trigger); }

//            ~PadInputModule()
//            {
//                ButtonInput = null;
//                TriggerInput = null;
//            }

//            public void SetEvent(eInputType inputType, Action<eInputType, Button, Index> buttonInput, Action<eInputType, Trigger, Index, bool, bool> triggerInput)
//            {
//                ButtonInput += (Button button, Index index) => { Gamepad.GetButton(inputType, button, index); };
//                TriggerInput += (Trigger trigger, Index index, bool currentInput, bool previewInput) => { Gamepad.GetTrigger(inputType, trigger, index, currentInput, previewInput); };
//            }

//            public Button GetButton() { return Button.Value; }
//            public Trigger GetTrigger() { return Trigger.Value; }

//            public bool UseButton() { return Button.HasValue; }
//            public bool UseTrigger() { return Trigger.HasValue; }

//            public void SetButton(Button button)
//            {
//                Button = button;
//                Trigger = null;
//            }
//            public void SetTrigger(Trigger trigger)
//            {
//                Button = null;
//                Trigger = trigger;
//            }

//            public void Update(Index index)
//            {

//                if (UseButton())
//                {
//                    ButtonInput?.Invoke(GetButton(), index);
//                }
//                else
//                {
//                    isCurrentTriggerPressed = (GamepadInput.GamePad.GetTrigger(GetTrigger(), index) > 0);
//                    TriggerInput?.Invoke(GetTrigger(), index, isCurrentTriggerPressed, isPreviewTriggerPressed);
//                    isPreviewTriggerPressed = isCurrentTriggerPressed;
//                }
//            }
//        }

//    }
//}
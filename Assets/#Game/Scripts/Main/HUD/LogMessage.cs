using UnityEngine;
using TMPro;
using DG.Tweening;

[DisallowMultipleComponent]
public class LogMessage : MonoBehaviour
{
    public class LogMessagePresenter
    {
        public class MessageComponent
        {
            TextMeshPro textMesh = null;
            const float BasePos = -260f;
            const float AdjustPos = -500f;
            const float SlideValue = 40f;

            public bool IsActive()
            {
                return textMesh.IsActive();
            }

            public MessageComponent(TextMeshPro textMesh)
            {
                this.textMesh = textMesh;
            }

            public void Activate(string text)
            {
                textMesh.gameObject.SetActive(true);
                textMesh.color = Color.white;
                textMesh.text = text;
                textMesh.transform.localPosition = new Vector2(textMesh.transform.localPosition.x, AdjustPos);
            }

            public void Disable()
            {
                textMesh.gameObject.SetActive(false);
            }

            public void OverfloweMessage()
            {
                var tween = textMesh
                    .transform
                    .DOLocalMoveY(BasePos + SlideValue, 0.5f);

                tween.OnUpdate(() => update(tween.Duration() / tween.Elapsed()))
                    .OnComplete(Disable);

                void update(float elapsedRate)
                {
                    Color color = textMesh.color;
                    color.a -= elapsedRate;
                    textMesh.color = color;
                }

            }

            public void Slide(int slideNum)
            {
                textMesh.transform.DOLocalMoveY(BasePos - SlideValue * slideNum, 1f);
            }

        }

        MessageComponent[] messageParts = null;
        FixedSizeQueue<MessageComponent> outputMessage = new FixedSizeQueue<MessageComponent>(3);

        Tween tween = null;
        const float DisableSecond = 5f;

        public LogMessagePresenter(TextMeshPro[] textMesh)
        {
            int length = textMesh.Length;
            messageParts = new MessageComponent[length];
            for (int i = 0; i < length; i++)
            {
                messageParts[i] = new MessageComponent(textMesh[i]);
            }
        }

        public void Output(string text)
        {
            foreach (var message in messageParts)
            {
                if (!message.IsActive())
                {
                    Activete(message, text);
                    break;
                }
            }

            tween?.Kill();
            tween = DOVirtual.DelayedCall(DisableSecond, DisableAll);
        }

        void DisableAll()
        {
            foreach (var message in messageParts)
            {
                message.Disable();
            }
        }

        void Activete(MessageComponent message, string text)
        {
            message.Activate(text);
            var overfloweMessage = outputMessage.Enqueue(message);
            overfloweMessage?.OverfloweMessage();

            for (int i = 0; i < outputMessage.Count(); i++)
            {
                outputMessage[i].Slide(i);
            }
        }

    }


    static LogMessagePresenter presenter = null;

    void Awake()
    {
        var prefabs = GetComponentsInChildren<TextMeshPro>();
        foreach (var prefab in prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
        presenter = new LogMessagePresenter(prefabs);
    }

    public static void OutputLog(object value)
    {
        presenter.Output(value.ToString());
    }

}
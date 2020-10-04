using Febucci.UI;
using UnityEngine;

public class OneShotTextPlayer : MonoBehaviour
{
    [SerializeField] private TextAnimatorPlayer namePlate;
    [SerializeField] private TextAnimator namePlateAnimator;

    [SerializeField] private TextAnimatorPlayer message;
    [SerializeField] private TextAnimator messageAnimator;

    [SerializeField] private string name;

    private CanvasGroup canvasGroup;

    private readonly string namePlateMessage = $"<?{SpeakerEvents.NamePlateDisplayed}>";
    private readonly string speechLineEndMessage = $"<?{SpeakerEvents.SpeechLineEnd}>";
    private bool decreaseAlpha = false;
    private string messageText;

    private void Awake()
    {
        namePlateAnimator.onEvent += OnEvent;
        messageAnimator.onEvent += OnEvent;
    }

    private void OnDestroy()
    {
        namePlateAnimator.onEvent -= OnEvent;
        messageAnimator.onEvent -= OnEvent;
    }
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(decreaseAlpha && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.005f;
        }
    }

    public void PlayMessage(string message)
    {
        Debug.Log("message " + message);
        messageText = message;
        ShowNamePlate();
    }

    private void ShowNamePlate()
    {
        Debug.Log("show name " + name);
        namePlate.ShowText($"{name}{namePlateMessage}");
    }

    private void ShowMessage()
    {
        Debug.Log("show message" + messageText);
        message.ShowText($"{messageText}{speechLineEndMessage}");
    }

    void OnEvent(string message)
    {
        switch (message)
        {
            case SpeakerEvents.NamePlateDisplayed:
                canvasGroup.alpha = 1f;
                decreaseAlpha = false;
                ShowMessage();
                break;

            case SpeakerEvents.SpeechLineEnd:
                decreaseAlpha = true;
                break;
        }
    }
}

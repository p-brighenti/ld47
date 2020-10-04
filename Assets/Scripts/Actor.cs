using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    [Serializable] public class OnSpeechEndEvent : UnityEvent { }

    [SerializeField] private string name;
    [SerializeField] [TextArea(15, 20)] private string dialogue;
    [SerializeField] bool loopSpeech;
    [SerializeField] [TextArea(15, 20)] private string speech;
    [SerializeField] private OnSpeechEndEvent onSpeechEnd;
    [SerializeField] private Actor interlocutor;
    [SerializeField] private bool inDialogueMode;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private TextAnimatorPlayer namePlate;
    [SerializeField] private TextAnimator namePlateAnimator;
    [SerializeField] private AudioClip rewindClip;

    private TextAnimator textAnimator;
    private TextAnimatorPlayer textPlayer;
    private LevelValidator levelValidator;

    private readonly string speechLineEndMessage = $"<?{SpeakerEvents.SpeechLineEnd}>";
    private readonly string dialogueLineEndMessage = $"<?{SpeakerEvents.DialogueLineEnd}>";
    private readonly string namePlateMessage = $"<?{SpeakerEvents.NamePlateDisplayed}>";

    private string[] speechByLine;
    private string[] dialogueByLine;
    private int currentSpeechLine = 0;
    private int currentDialogueLine = 0;
    private bool visibleNamePlate = false;

    private void Awake()
    {
        textAnimator = GetComponent<TextAnimator>();
        namePlateAnimator.onEvent += OnEvent;
        textAnimator.onEvent += OnEvent;
    }

    private void OnDestroy()
    {
        namePlateAnimator.onEvent -= OnEvent;
        textAnimator.onEvent -= OnEvent;
    }

    void Start()
    {
        textPlayer = GetComponent<TextAnimatorPlayer>();
        levelValidator = GameObject.FindObjectOfType<LevelValidator>();

        if (inDialogueMode)
        {
            dialogueByLine = dialogue.Split('\n');
            return;
        }

        speechByLine = speech.Split('\n');
        ShowNamePlate();
    }

    public void PlayNextDialogueLine()
    {
        if (!visibleNamePlate)
        {
            visibleNamePlate = true;
            ShowNamePlate();
            return;
        }

        PlayDialogueLine(currentDialogueLine);

        if (currentDialogueLine >= dialogueByLine.Length - 1)
        {
            currentDialogueLine++;
            dialogueManager.incrementInterlocutorDone();
            return;
        }

        currentDialogueLine++;
    }

    private void PlaySpeechLine(int index)
    {
        textPlayer.ShowText(speechByLine[index] + speechLineEndMessage);
    }


    private void PlayDialogueLine(int index)
    {
        if (index >= dialogueByLine.Length)
        {
            Debug.Log("invalid dialogue line");
            return;
        }
        textPlayer.ShowText($"{dialogueByLine[index]}{dialogueLineEndMessage}");
    }

    void OnEvent(string message)
    {
        switch (message)
        {
            case SpeakerEvents.SpeechLineEnd:
                OnSpeechLineEnd();
                break;

            case SpeakerEvents.DialogueLineEnd:
                OnDialogueLineEnd();
                break;

            case SpeakerEvents.NamePlateDisplayed:
                if (inDialogueMode)
                {
                    PlayNextDialogueLine();
                    return;
                }
                PlaySpeechLine(currentSpeechLine);
                break;

            default:
                Debug.LogError($"INVALID SpeechHandler EVENT: \"{message}\"");
                break;
        }
    }

    private void OnDialogueLineEnd()
    {
        interlocutor.PlayNextDialogueLine();
    }

    private void OnSpeechLineEnd()
    {
        currentSpeechLine++;

        if (currentSpeechLine < speechByLine.Length)
        {
            PlaySpeechLine(currentSpeechLine);
            return;
        }

        if (loopSpeech)
        {
            GetComponentInChildren<AudioSource>().PlayOneShot(rewindClip);
            StartCoroutine(LoopBack());
            return;
        }

        if (onSpeechEnd != null)
        {
            onSpeechEnd.Invoke();
        }
    }

    private IEnumerator LoopBack()
    {
        yield return new WaitForSecondsRealtime(rewindClip.length);

        currentSpeechLine = 0;
        PlaySpeechLine(currentSpeechLine);

        if (levelValidator != null)
        {
            levelValidator.EnableAnswerBox();
        }
    }

    private void ShowNamePlate()
    {
        namePlate.ShowText($"{name}{namePlateMessage}");
    }
}

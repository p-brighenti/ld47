using Febucci.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelValidator : MonoBehaviour
{
    [SerializeField] private int currentTape;
    [SerializeField] private CanvasGroup answerBoxUI;
    [SerializeField] private TextMeshProUGUI answerBoxText;
    [SerializeField] private ValidatorFeedback feedback;
    [SerializeField] private OneShotTextPlayer feedbackPlayer;
    [SerializeField] private CanvasGroup nextLevelUI;

    private string[] tapeSolutions = { "familyjewels", "2324", "thankyou" };
    private System.Random rng;

    private void Start()
    {
        rng = new System.Random();
    }

    public void EnableAnswerBox()
    {
        answerBoxUI.alpha = 1f;
        answerBoxUI.interactable = true;
        answerBoxUI.blocksRaycasts = true;
    }

    public void ValidateSolution()
    {
        string userInput = answerBoxText.text.Remove(answerBoxText.text.Length - 1);
        string answer = new string(userInput.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());

        if (tapeSolutions[currentTape - 1].Equals(answer))
        {
            feedbackPlayer.PlayMessage(feedback.onSuccess[rng.Next(feedback.onSuccess.Length)]);
            RevealNextLevelUI();
        }
        else
        {
            feedbackPlayer.PlayMessage(feedback.onFail[rng.Next(feedback.onFail.Length)]);
        }
    }

    private void RevealNextLevelUI()
    {
        nextLevelUI.alpha = 1f;
        nextLevelUI.interactable = true;
        nextLevelUI.blocksRaycasts = true;
    }
}

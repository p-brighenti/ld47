using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Serializable] public class OnDialogueEndEvent : UnityEvent { }

    [SerializeField] private Actor firstInterlocutor;
    [SerializeField] private Actor secondInterlocutor;
    [SerializeField] private OnDialogueEndEvent onEndEvent;
    
    private int doneSpeakingCount = 0;

    private void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        firstInterlocutor.PlayNextDialogueLine();
    }

    public void incrementInterlocutorDone()
    {
        doneSpeakingCount++;
        Debug.Log("incrementing");
        
        if(doneSpeakingCount >= 2)
        {
            Debug.Log("Calling event");
            onEndEvent.Invoke();
        }
    }
}

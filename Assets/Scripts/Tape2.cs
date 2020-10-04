using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tape2 : MonoBehaviour
{
    [Serializable] public class OnLoopEvent : UnityEvent { }

    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip changeDirectionLeft;
    [SerializeField] private AudioClip changeDirectionRight;
    [SerializeField] private AudioClip endOfTape;
    [SerializeField] private LevelValidator levelValidator;

    private AudioSource audioPlayer;

    private bool looped = false;
    private bool solved;
    private Directions[] thiefPath = {
        Directions.Front,
        Directions.Front,
        Directions.Left,
        Directions.Front,
        Directions.Right,
        Directions.Front,
        Directions.Left,
        Directions.Left,
        Directions.Front,
        Directions.Left,
        Directions.Front,
        Directions.Left,
        Directions.Front,
        Directions.Right,
        Directions.Front,
        Directions.Right,
        Directions.Front
    };

    private enum Directions
    {
        Front,
        Left,
        Right
    }

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        StartCoroutine(PlayTape());
    }

    public IEnumerator PlayTape()
    {
       audioPlayer.PlayOneShot(endOfTape);

        yield return new WaitForSecondsRealtime(1f);

        while (!solved)
        {
            foreach (Directions d in thiefPath)
            {
                switch (d)
                {
                    case Directions.Front:
                        PlayForwardStep();
                        break;

                    case Directions.Left:
                        PlayLeftStep();
                        break;

                    case Directions.Right:
                        PlayRightStep();
                        break;
                }

                yield return new WaitForSecondsRealtime(1f);
            }

            if (!looped)
            {
                looped = true;
                levelValidator.EnableAnswerBox();
            }

            audioPlayer.PlayOneShot(endOfTape);
            yield return new WaitForSecondsRealtime(endOfTape.length);
        }
    }

    public void markAsSolved()
    {
        solved = true;
    }

    private void PlayForwardStep()
    {
        audioPlayer.PlayOneShot(stepSound);
    }

    private void PlayLeftStep()
    {
        audioPlayer.PlayOneShot(changeDirectionLeft);
    }

    private void PlayRightStep()
    {
        audioPlayer.PlayOneShot(changeDirectionRight);
    }
}

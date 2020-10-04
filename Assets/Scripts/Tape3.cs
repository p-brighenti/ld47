using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape3 : MonoBehaviour
{

    [SerializeField] private AudioClip shortMorse;
    [SerializeField] private AudioClip longMorse;
    [SerializeField] private AudioClip endOfTape;
    [SerializeField] private LevelValidator levelValidator;

    private AudioSource audioPlayer;

    private bool looped = false;
    private bool solved = false;
    private int[][] morseMessage = {
        MorseCode.W,
        MorseCode.K,
        MorseCode.D,
        MorseCode.Q,
        MorseCode.N,
        MorseCode.B,
        MorseCode.R,
        MorseCode.X
    };

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        StartCoroutine(PlayMorseMessage());
    }

    private IEnumerator PlayMorseMessage()
    {
        while (!solved)
        {
            audioPlayer.PlayOneShot(endOfTape);

            yield return new WaitForSecondsRealtime(endOfTape.length + 0.5f);

            foreach (int[] letter in morseMessage)
            {
                foreach (int signal in letter)
                {
                    var signalSound = signal == 1 ? shortMorse : longMorse;

                    audioPlayer.PlayOneShot(signalSound);
                    yield return new WaitForSecondsRealtime(signalSound.length + 0.4f);
                }

                yield return new WaitForSecondsRealtime(1.5f);
            }

            if (!looped)
            {
                looped = true;
                levelValidator.EnableAnswerBox();
            }
        }

    }
}

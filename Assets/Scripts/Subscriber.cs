using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subscriber : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject managedObject;
    private bool fadeOutCanvas = false;

    private void Update()
    {
        if (fadeOutCanvas && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.007f;
        }

        if(canvasGroup.alpha == 0 && !managedObject.active)
        {
            managedObject.SetActive(true);
        }
    }

    public void RevealCanvasGroup()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void FadeOutCanvasGroup()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        fadeOutCanvas = true;
    }
}

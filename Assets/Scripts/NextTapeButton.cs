using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextTapeButton : MonoBehaviour
{
    void Start()
    {
        var label = SceneManager.GetActiveScene().buildIndex != 5 ? "<rainb>Next tape!</rainb>" : "<shake>All done!</shake>";
        GetComponentInChildren<TextAnimatorPlayer>().ShowText(label);
    }
}

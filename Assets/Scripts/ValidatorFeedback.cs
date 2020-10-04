using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValidatorFeedback", menuName = "ScriptableObjects/ValidatorFeedback", order = 1)]
public class ValidatorFeedback : ScriptableObject 
{
    public string[] onSuccess;
    public string[] onFail;
}

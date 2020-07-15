using Stupid.UwU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldoutSetactiveExample : MonoBehaviour
{
    [FoldOut]
    public bool toggleMe = false;

    [SetActive("toggleMe"), HelpBox(1, MessageType.Error)]
    public string shrug = Funny.shrug;
}

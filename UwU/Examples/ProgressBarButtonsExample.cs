using Stupid.UwU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarButtonsExample : MonoBehaviour
{
    [BeginHorizontal, Button("One")]
    public bool one;
    [EndHorizontal, Button("Two")]
    public bool two;

    [ProgressBar(0, 100)]
    public float progress;

    private void Update()
    {
        progress += Time.deltaTime;
    }
}

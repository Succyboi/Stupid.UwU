using Stupid.UwU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpboxExample : MonoBehaviour
{
    [HelpBox]
    public string helpbox1 = "Hey I'm a helpbox";
    [HelpBox(1, MessageType.Info)]
    public string helpbox2 = "I can show messages...";
    [HelpBox(1, MessageType.Warning)]
    public string helpbox3 = "Warnings...";
    [HelpBox(1, MessageType.Error)]
    public string helpbox4 = "And errors!";
}

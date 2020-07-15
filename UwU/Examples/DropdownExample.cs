using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stupid.UwU;

[ExecuteAlways, AddComponentMenu("UwU/Dropdown Example")]
public class DropdownExample : MonoBehaviour
{
    [Dropdown(new object[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
    public string DropDown0 = "Option 1";
}

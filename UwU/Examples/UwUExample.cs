using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stupid.UwU;

[ExecuteAlways,  AddComponentMenu("UwU/UwU Showcase")]
public class UwUExample : MonoBehaviour
{
    //LEFT SIDE QA
    [Line, BeginHorizontal, BeginVertical, Title("Question: ", 1.75f, LabelStyle.bold, TextSize.larger)]
    [Label(2.35f, LabelStyle.regular, TextSize.larger)]
    public string What = "'What is UwU?'";
    [Label(2.35f, LabelStyle.regular, TextSize.larger)]
    public string HowMuch = "'Why would I use UwU?'";
    [Label(2.35f, LabelStyle.regular, TextSize.larger), EndVertical]
    public string HowToGet = "'Where can I get UwU?'";


    //RIGHT SIDE QA
    [Line, BeginVertical, Title("Answer: ", 1.75f, LabelStyle.bold, TextSize.larger), HelpBox(2.5f, MessageType.None)]
    public string description1 = "UwU is a super easy to use library for Unity that enables you to make your scripts look as beautiful as ever WITHOUT having to write a custom editor";

    [HelpBox(2.5f, MessageType.None)]
    public string description2 = "With UwU you can make your scripts in Unity look better, be easier to use and have nearly all the benefits of a custom inspector but without the hassle!";

    [EndHorizontal, EndVertical, HelpBox(2.5f, MessageType.None)]
    public string description3 = "On GitHub! You can find it here:\nhttps://github.com/Succyboi/Stupid.UwU";


    //FEATURES
    [Line, Title("FEATURES:", 1.5f, LabelStyle.bold, TextSize.larger), Label]
    public string FeatureDescription = "UwU includes...";

    //labels and styles
    [Space, BeginHorizontal, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string LabelStyles = "Labels and styles";
        [Label(1, LabelStyle.bold, TextSize.normal)]
    public string Style0 = "Wow";
        [Label(0.75f, LabelStyle.white, TextSize.small)]
    public string Style1 = "Such Labels";
        [EndVertical, Label(0.5f, LabelStyle.mini, TextSize.finePrint)]
    public string Style2 = "Much text, such wow";

    //dropdowns
    [Space, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string Dropdowns = "Dropdowns";
        [Dropdown(new object[] {"Wow", "Wow", "Wow", "Wow", "AMAZING"})]
    public string DropDown0 = "Wow";
        [EndHorizontal, EndVertical, Dropdown(new object[] { "Wow", "Wow", "Wow", "Wow", "AMAZING" })]
    public string DropDown1 = "AMAZING";


    //foldouts
    [Space, BeginHorizontal, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string Foldouts = "(Fully functional) Foldouts";
        [FoldOut("Open Me!!!")]
    public bool foldout0 = true;
        [EndVertical, SetActive("foldout0"), HelpBox(3.5f)]
    public string foldoutText = "Blablablablablablablablablablablablablablablablablabalalbalbalablabalbakfsadjkjhfdaskljhfalskhfskadhjkjdhafsjhkasdhkjsajkjhadfsjhklasdfjkhlasdfjhklsjhahjklafslgyiwbfiuhndsalkfhbjhdyasfbjhubljhbwf";

    //helpboxes
    [Space, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string HelpBoxes = "HelpBoxes";
        [HelpBox(1.5f, MessageType.Info)]
    public string HelpBox0 = "Oh no please help!";
        [HelpBox(1.5f, MessageType.Warning)]
    public string HelpBox1 = "Help I'm dying UnU!!!";
        [EndHorizontal, EndVertical, HelpBox(1.5f, MessageType.Error)]
    public string HelpBox2 = "XmX bleh. </3";


    //foldouts
    [Space, BeginHorizontal, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string Buttons = "BUTTONS!!! (Yes they work)";
        [Button("anything", "CLICK ME")]
    public bool button0 = true;
        [Button("anything", "NO CLICK ME")]
    public bool button1 = true;
        [EndVertical, Button("anything", "NO NO ME! Click MEEEEE")]
    public bool button2 = true;

    //jokes
    [Space, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string Jokes = "Generated dad jokes";
        [Label(1, LabelStyle.mini)]
    public string Joke0 = "What kind of award did the dentist receive? A little plaque.";
        [Label(1, LabelStyle.mini)]
    public string Joke1 = "What's blue and not very heavy?  Light blue.";
        [EndHorizontal, EndVertical, Label(1, LabelStyle.mini)]
    public string Joke2 = "The shovel was a ground-breaking invention.";


    //progress bars
    [Space, BeginHorizontal, BeginVertical, Label(1.25f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string ProgressBars = "ProgressBars";
    [ProgressBar(0, 100)]
    public float progressBar0 = 0;
    [ProgressBar(0, 100)]
    public float progressBar1 = 0;
    [EndVertical, ProgressBar(0, 100)]
    public float progressBar2 = 0;

    //jokes
    [Space, EndHorizontal, Label(2.5f, LabelStyle.miniCenteredGray, TextSize.larger)]
    public string bullShit = "And a whole lot of other\n uneccesary bullshit";



    //LISENCE
    [Line, Title("Lisence:", 1.5f, LabelStyle.bold, TextSize.larger), FoldOut("Show Lisence (MIT)")]
    public bool showLisence = false;

    [SetActive("showLisence"), HelpBox(5, MessageType.None)]
    public string FinePrint = "Copyright 2020 Pelle Bruinsma\nPermission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the 'Software'), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\nTHE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

    public void UwULink()
    {
        System.Diagnostics.Process.Start("http://stupidplusplus.com");
    }

    private void Update()
    {
        progressBar0 += Time.deltaTime * 50;
        progressBar1 += Time.deltaTime * 30;
        progressBar2 += Time.deltaTime * 20;

        if(progressBar0 > 100)
        {
            progressBar0 = 0;
        }

        if (progressBar1 > 100)
        {
            progressBar1 = 0;
        }

        if (progressBar2 > 100)
        {
            progressBar2 = 0;
        }
    }
}

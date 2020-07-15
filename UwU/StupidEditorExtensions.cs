using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Net;

namespace Stupid
{
    namespace UwU
    {

        /*
         * Yet to add:
         * https://docs.unity3d.com/ScriptReference/EditorGUI.DropdownButton.html
         * https://docs.unity3d.com/ScriptReference/EditorGUI.BeginFoldoutHeaderGroup.html
         * https://docs.unity3d.com/ScriptReference/EditorGUI.InspectorTitlebar.html
         * https://docs.unity3d.com/ScriptReference/EditorGUI.DrawPreviewTexture.html
         */

#if UNITY_EDITOR

        //Custom inspector that we make by default for basically everything
        [CustomEditor(typeof(UnityEngine.Object), true), CanEditMultipleObjects]
        public class ObjectEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                //update 
                serializedObject.Update();

                FieldInfo[] fields = this.target.GetType().GetFields();

                //Loop through all properties
                SerializedProperty prop = serializedObject.GetIterator();
                if (prop.NextVisible(true))
                {
                    while (prop.NextVisible(false))
                    {
                        FieldInfo fieldInfo = serializedObject.FindProperty(prop.name).GetMatchingField(fields);

                        //begin horizontal
                        if (fieldInfo != null)
                        {
                            BeginHorizontalAttribute beginHorizontalAttribute = fieldInfo.GetCustomAttribute<BeginHorizontalAttribute>();

                            //and it has the attribute
                            if (beginHorizontalAttribute != null)
                            {
                                //begin horizontal
                                EditorGUILayout.BeginHorizontal();
                            }
                        }

                        //begin vertical
                        if (fieldInfo != null)
                        {
                            BeginVerticalAttribute beginVerticalAttribute = fieldInfo.GetCustomAttribute<BeginVerticalAttribute>();

                            //and it has the attribute
                            if (beginVerticalAttribute != null)
                            {
                                //begin horizontal
                                EditorGUILayout.BeginVertical();
                            }
                        }

                        //EXTRA STUFF FOR THE SETACTIVE
                        if (fieldInfo != null)
                        {
                            SetActiveAttribute setActiveAttribute = fieldInfo.GetCustomAttribute<SetActiveAttribute>();

                            //and it has the attribute
                            if (setActiveAttribute != null)
                            {
                                bool enabled = GUIHelper.GetConditionalHideAttributeResult(setActiveAttribute, serializedObject.FindProperty(prop.name));

                                if (!setActiveAttribute.HideInInspector || enabled)
                                {
                                    //Enable/disable the property
                                    bool wasEnabled = GUI.enabled;
                                    GUI.enabled = enabled;

                                    //draw the default
                                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);

                                    //Ensure that the next property that is being drawn uses the correct settings
                                    GUI.enabled = wasEnabled;
                                }
                            }
                            else
                            {
                                //draw the default
                                EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                            }
                        }
                        else
                        {
                            //draw the default
                            EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                        }

                        //tying buttons to functions
                        if (fieldInfo != null)
                        {
                            ButtonAttribute buttonAttribute = fieldInfo.GetCustomAttribute<ButtonAttribute>();

                            //and it has the attribute
                            if (buttonAttribute != null)
                            {
                                //if it is a boolean
                                if (serializedObject.FindProperty(prop.name).propertyType == SerializedPropertyType.Boolean)
                                {
                                    //and it is marked true (which means it's been pressed)
                                    if (serializedObject.FindProperty(prop.name).boolValue == true)
                                    {
                                        //find and call the button's method by name
                                        var methods = this.target.GetType()
                                            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                                            .Where(m => m.GetParameters().Length == 0);

                                        foreach(var method in methods)
                                        {
                                            //if we've found a matching method name
                                            if(buttonAttribute.functionName == method.Name)
                                            {
                                                foreach (var t in this.targets)
                                                {
                                                    method.Invoke(t, null);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //end vertical
                        if (fieldInfo != null)
                        {
                            EndVerticalAttribute endVerticalAttribute = fieldInfo.GetCustomAttribute<EndVerticalAttribute>();

                            //and it has the attribute
                            if (endVerticalAttribute != null)
                            {
                                //begin horizontal
                                EditorGUILayout.EndVertical();
                            }
                        }

                        //end horizontal
                        if (fieldInfo != null)
                        {
                            EndHorizontalAttribute endHorizontalAttribute = fieldInfo.GetCustomAttribute<EndHorizontalAttribute>();

                            //and it has the attribute
                            if (endHorizontalAttribute != null)
                            {
                                //begin horizontal
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                }

                //apply potential changes
                serializedObject.ApplyModifiedProperties();
            }
        }



        #region Gui Helper Functions

        static class GUIHelper
        {
            public static GUIStyle GUIStyleFromLabelStyle(LabelStyle labelType)
            {
                switch (labelType)
                {
                    case LabelStyle.regular:
                        return EditorStyles.label;

                    case LabelStyle.bold:
                        return EditorStyles.boldLabel;

                    case LabelStyle.large:
                        return EditorStyles.largeLabel;

                    case LabelStyle.link:
                        return EditorStyles.linkLabel;

                    case LabelStyle.mini:
                        return EditorStyles.miniLabel;

                    case LabelStyle.miniBold:
                        return EditorStyles.miniBoldLabel;

                    case LabelStyle.miniCenteredGray:
                        return EditorStyles.centeredGreyMiniLabel;

                    case LabelStyle.white:
                        return EditorStyles.whiteLabel;

                    case LabelStyle.whiteBold:
                        return EditorStyles.whiteBoldLabel;

                    case LabelStyle.whiteLarge:
                        return EditorStyles.whiteLargeLabel;

                    case LabelStyle.whiteMini:
                        return EditorStyles.whiteMiniLabel;

                    default:
                        return null;
                }
            }

            public static UnityEditor.MessageType UnityMessageTypeConverter(MessageType messageType)
            {
                switch (messageType)
                {
                    case MessageType.None:
                        return UnityEditor.MessageType.None;

                    case MessageType.Info:
                        return UnityEditor.MessageType.Info;

                    case MessageType.Warning:
                        return UnityEditor.MessageType.Warning;

                    case MessageType.Error:
                        return UnityEditor.MessageType.Error;

                    default:
                        return UnityEditor.MessageType.None;
                }
            }

            public static FieldInfo GetMatchingField(this SerializedProperty serializedProperty, FieldInfo[] fields)
            {
                foreach (FieldInfo field in fields)
                {
                    //if these match
                    if (field.Name == serializedProperty.name)
                    {
                        return field;
                    }
                }

                return null;
            }

            public static bool GetConditionalHideAttributeResult(SetActiveAttribute setActiveAtt, SerializedProperty property)
            {
                bool enabled = true;
                //Look for the sourcefield within the object that the property belongs to
                string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
                string conditionPath = propertyPath.Replace(property.name, setActiveAtt.basedOnField); //changes the path to the conditionalsource property path
                SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

                if (sourcePropertyValue != null)
                {
                    enabled = sourcePropertyValue.boolValue;
                }
                else
                {
                    Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + setActiveAtt.basedOnField);
                }

                return enabled;
            }
        }

        #endregion

#endif

        #region Enumerables
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error
        }
        public enum TextSize : byte
        {
            normal,
            finePrint = 5,
            small = 10,
            medium = 12,
            large = 13,
            larger = 20,
            huge = 30,
            enormous = 50
        }

        public enum LabelStyle
        {
            regular,
            bold,
            large,
            link,
            mini,
            miniBold,
            miniCenteredGray,
            white,
            whiteBold,
            whiteLarge,
            whiteMini
        }
        #endregion

        #region Property attributes
        // Attribute used to display a line above the property
        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class LineAttribute : PropertyAttribute
        {
            public readonly int height;

            public LineAttribute(int height = 1)
            {
                this.height = height;
            }
        }

        // Attribute used to display a string as a label field
        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class LabelAttribute : PropertyAttribute
        {
            public float overrideHeight;
            public LabelStyle style;
            public TextSize textSize;

            public LabelAttribute(float overrideHeight = 1, LabelStyle style = LabelStyle.regular, TextSize textSize = TextSize.normal)
            {
                this.overrideHeight = overrideHeight;
                this.style = style;
                this.textSize = textSize;
            }
        }

        // Attribute used to display a string as a label field
        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class TitleAttribute : PropertyAttribute
        {
            public string content;
            public float overrideHeight;
            public LabelStyle style;
            public TextSize textSize;

            public TitleAttribute(string content, float overrideHeight = 1, LabelStyle style = LabelStyle.bold, TextSize textSize = TextSize.normal)
            {
                this.content = content;
                this.overrideHeight = overrideHeight;
                this.style = style;
                this.textSize = textSize;
            }
        }

        // Attribute used to display a string dropdown
        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class DropdownAttribute : PropertyAttribute
        {
            public readonly object[] options;

            public DropdownAttribute(object[] options)
            {
                this.options = options;
            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class HelpBoxAttribute : PropertyAttribute
        {
            public MessageType msgType;
            public float overrideHeight;

            public HelpBoxAttribute(float overrideHeight = 1, MessageType msgType = MessageType.None)
            {
                this.overrideHeight = overrideHeight;
                this.msgType = msgType;
            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class SetActiveAttribute : PropertyAttribute
        {
            public string basedOnField;
            public bool HideInInspector = true;

            public SetActiveAttribute(string basedOnField, bool HideInInspector = true)
            {
                this.basedOnField = basedOnField;
                this.HideInInspector = HideInInspector;
            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class FoldOutAttribute : PropertyAttribute
        {
            public string title;
            public FoldOutAttribute(string title = "")
            {
                this.title = title;
            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class ProgressBarAttribute : PropertyAttribute
        {
            public float leftBound;
            public float rightBound;
            public ProgressBarAttribute(float leftBound, float rightBound)
            {
                this.leftBound = leftBound;
                this.rightBound = rightBound;
            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class BeginHorizontalAttribute : PropertyAttribute
        {
            public BeginHorizontalAttribute()
            {

            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class EndHorizontalAttribute : PropertyAttribute
        {
            public EndHorizontalAttribute()
            {

            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class BeginVerticalAttribute : PropertyAttribute
        {
            public BeginVerticalAttribute()
            {

            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class EndVerticalAttribute : PropertyAttribute
        {
            public EndVerticalAttribute()
            {

            }
        }

        [System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        public sealed class ButtonAttribute : PropertyAttribute
        {
            public string functionName;
            public string textOnButton;
            public float overrideHeight;
            public ButtonAttribute(string functionName, string textOnButton = "", float overrideHeight = 1)
            {
                this.functionName = functionName;
                this.textOnButton = textOnButton;
                this.overrideHeight = overrideHeight;
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Property drawers

        [CustomPropertyDrawer(typeof(LineAttribute))]
        internal sealed class LineDrawer : DecoratorDrawer
        {
            public override void OnGUI(Rect position)
            {
                LineAttribute line = (LineAttribute)attribute;

                position.height = line.height;

                EditorGUI.DrawRect(position, new Color(0.5f, 0.5f, 0.5f, 1));
            }
            public override float GetHeight()
            {
                LineAttribute line = (LineAttribute)attribute;

                return line.height + 2;
            }
        }

        [CustomPropertyDrawer(typeof(LabelAttribute))]
        internal sealed class LabelDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                LabelAttribute labelAtt = (LabelAttribute)attribute;

                string content;

                if (property.propertyType == SerializedPropertyType.String)
                {
                    content = property.stringValue;
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    content = property.intValue.ToString();
                }
                else if (property.propertyType == SerializedPropertyType.Float)
                {
                    content = property.floatValue.ToString();
                }
                else
                {
                    content = "Use Label with string, int and float values only.";
                }

                GUIStyle style = GUIHelper.GUIStyleFromLabelStyle(labelAtt.style);

                //store font size before
                int fontSize = style.fontSize;

                //set size
                if(labelAtt.textSize != TextSize.normal)
                {
                    style.fontSize = (int)labelAtt.textSize;
                }

                EditorGUI.LabelField(position, content, style);

                //set size
                if (labelAtt.textSize != TextSize.normal)
                {
                    style.fontSize = fontSize;
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                LabelAttribute labelAtt = (LabelAttribute)attribute;

                return EditorGUI.GetPropertyHeight(property, label) * labelAtt.overrideHeight;
            }
        }

        [CustomPropertyDrawer(typeof(TitleAttribute))]
        internal sealed class TitleDrawer : DecoratorDrawer
        {
            public override void OnGUI(Rect position)
            {
                TitleAttribute title = (TitleAttribute)attribute;

                position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
                position = EditorGUI.IndentedRect(position);

                GUIStyle style = GUIHelper.GUIStyleFromLabelStyle(title.style);

                //store font size before
                int fontSize = style.fontSize;

                //set size
                if (title.textSize != TextSize.normal)
                {
                    style.fontSize = (int)title.textSize;
                }

                EditorGUI.LabelField(position, title.content, style);

                //set size
                if (title.textSize != TextSize.normal)
                {
                    style.fontSize = fontSize;
                }
            }

            public override float GetHeight()
            {
                TitleAttribute title = (TitleAttribute)attribute;

                return EditorGUIUtility.singleLineHeight * title.overrideHeight + EditorGUIUtility.singleLineHeight * 0.5f;
            }
        }

        [CustomPropertyDrawer(typeof(DropdownAttribute))]
        internal sealed class DropDownDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                DropdownAttribute dropdown = (DropdownAttribute)attribute;

                //based on property type
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        string[] displayedOptions = dropdown.options.Select(x => x.ToString()).ToArray();

                        int match = 0;

                        for (int o = 0; o < dropdown.options.Length; o++)
                        {
                            if (property.intValue == (int)dropdown.options[o])
                            {
                                //found match in options
                                match = o;
                            }
                        }

                        property.intValue = (int)dropdown.options[EditorGUI.Popup(position, match, displayedOptions)];
                        break;

                    case SerializedPropertyType.Boolean:
                        displayedOptions = dropdown.options.Select(x => x.ToString()).ToArray();

                        match = 0;

                        for (int o = 0; o < dropdown.options.Length; o++)
                        {
                            if (property.boolValue == (bool)dropdown.options[o])
                            {
                                //found match in options
                                match = o;
                            }
                        }

                        property.boolValue = (bool)dropdown.options[EditorGUI.Popup(position, match, displayedOptions)];
                        break;

                    case SerializedPropertyType.Float:
                        displayedOptions = dropdown.options.Select(x => x.ToString()).ToArray();

                        match = 0;

                        for (int o = 0; o < dropdown.options.Length; o++)
                        {
                            if (property.floatValue == (float)dropdown.options[o])
                            {
                                //found match in options
                                match = o;
                            }
                        }

                        property.floatValue = (float)dropdown.options[EditorGUI.Popup(position, match, displayedOptions)];
                        break;

                    case SerializedPropertyType.String:
                        displayedOptions = dropdown.options.Select(x => x.ToString()).ToArray();

                        match = 0;

                        for (int o = 0; o < dropdown.options.Length; o++)
                        {
                            if (property.stringValue == (string)dropdown.options[o])
                            {
                                //found match in options
                                match = o;
                            }
                        }

                        property.stringValue = (string)dropdown.options[EditorGUI.Popup(position, match, displayedOptions)];
                        break;

                    default:
                        EditorGUI.LabelField(position, "Use Dropdown with int, bool, float or string values only.");
                        break;
                }
            }
        }

        [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
        internal sealed class HelpBoxDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                HelpBoxAttribute helpBox = (HelpBoxAttribute)attribute;

                string content = "";

                if (property.propertyType == SerializedPropertyType.String)
                {
                    content = property.stringValue;
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    content = property.intValue.ToString();
                }
                else if (property.propertyType == SerializedPropertyType.Float)
                {
                    content = property.floatValue.ToString();
                }
                else
                {
                    content = "Use helpbox with string, int and float values only.";
                }

                EditorGUI.HelpBox(position, content, GUIHelper.UnityMessageTypeConverter(helpBox.msgType));
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                HelpBoxAttribute helpBox = (HelpBoxAttribute)attribute;

                return EditorGUI.GetPropertyHeight(property, label) * helpBox.overrideHeight;
            }
        }

        [CustomPropertyDrawer(typeof(FoldOutAttribute))]
        internal sealed class FoldoutDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                FoldOutAttribute foldOut = (FoldOutAttribute)attribute;

                string title = foldOut.title;

                if(title == "")
                {
                    title = property.displayName;
                }

                property.boolValue = EditorGUI.Foldout(position, property.boolValue, title);
            }
        }

        [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
        internal sealed class ProgressBarDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                ProgressBarAttribute progressBar = (ProgressBarAttribute)attribute;

                float progress = 0;

                if (property.propertyType == SerializedPropertyType.Integer)
                {
                    progress = Mathf.Clamp01(((float)property.intValue - progressBar.leftBound) / progressBar.rightBound);
                }
                else if (property.propertyType == SerializedPropertyType.Float)
                {
                    progress = Mathf.Clamp01((property.floatValue - progressBar.leftBound) / progressBar.rightBound);
                }
                else
                {
                    GUI.Label(position, "Use ProgressBar with int and float values only.");
                    return;
                }

                EditorGUI.ProgressBar(position, progress, property.displayName);
            }
        }

        [CustomPropertyDrawer(typeof(ButtonAttribute))]
        internal sealed class ButtonDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                ButtonAttribute button = (ButtonAttribute)attribute;

                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    string title = button.textOnButton;

                    if (title == "")
                    {
                        title = property.displayName;
                    }

                    property.boolValue = GUI.Button(position, title);
                }
                else
                {
                    GUI.Label(position, "Use Button with boolean values only.");
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                ButtonAttribute button = (ButtonAttribute)attribute;

                return EditorGUI.GetPropertyHeight(property, label) * button.overrideHeight;
            }
        }
        #endregion
#endif

        #region Misc
        public static class Funny 
        {
            #region Private fields
            private static string[] lorumIpsumWords = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
            #endregion

            public static string lorumIpsum(int words)
            {
                string result = "";

                for(int w = 0; w < words; w++)
                {
                    //pick random work and add to result
                    result += lorumIpsumWords[Mathf.RoundToInt(UnityEngine.Random.value * lorumIpsumWords.Length)];
                }

                return result;
            }

            public static string dadJoke()
            {
                WebClient client = new WebClient();

                client.Headers.Clear();
                client.Headers.Add("accept", "text/plain");
                string vits = client.DownloadString("https://icanhazdadjoke.com/");

                return vits;
            }

            public static string shrug = @"¯\_(ツ)_/¯";

            public static string flip = @"(╯°□°）╯︵ ┻━┻";

            public static string dino =     "───────────████████\n" +
                                            "──────────███▄███████\n" +
                                            "──────────███████████\n" +
                                            "──────────███████████\n" +
                                            "──────────██████\n" +
                                            "──────────█████████\n" +
                                            "█───────███████\n" +
                                            "██────████████████\n" +
                                            "███──██████████──█\n" +
                                            "███████████████\n" +
                                            "███████████████\n" +
                                            "─█████████████\n" +
                                            "──███████████\n" +
                                            "────████████\n" +
                                            "─────███──██\n" +
                                            "─────██────█\n" +
                                            "─────█─────█\n" +
                                            "─────██────██";
        }
        #endregion

    }
}
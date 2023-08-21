using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using System.IO;


/*
 * Some links
 * https://stackoverflow.com/questions/3791103/c-sharp-continuously-read-file
 * 
 */


namespace It4080
{
    public class It4080TheWindow : EditorWindow {
        string myString = "Hello World";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;

        public It4080TheWindow() {
        }


        public void CreateGUI() {
            Label lbl = new Label("Hello");
            rootVisualElement.Add(lbl);

            TextField txt = new TextField();
            txt.multiline = true;           
            rootVisualElement.Add(txt);
            txt.value = FileToText("/Users/profbutch/temp/unity_builds/TheBuild_1.log");
        }


        void OnGUI() {
            //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            //myString = EditorGUILayout.TextField("Text Field", myString);

            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();
        }


        private string FileToText(string path)
        {
            StreamReader reader = new StreamReader(path);
            string toReturn = reader.ReadToEnd();
            reader.Close();
            return toReturn;
        }
    }

}

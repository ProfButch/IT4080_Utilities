using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

namespace It4080
{
    public class LogViewer : EditorWindow
    {

        private Label txtLog1;
        private Label txtLog2;


        //[MenuItem("Window/UI Toolkit/LogViewer")]
        //public static void ShowExample()
        //{
        //    LogViewer wnd = GetWindow<LogViewer>();
        //    wnd.titleContent = new GUIContent("LogViewer");
        //}


        private void OutOfTheBoxGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            //VisualElement label = new Label("Hello World! From C#");
            //root.Add(label);

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/IT4080/Editor/LogViewer.uxml");
            VisualElement uxmlElements = visualTree.Instantiate();
            root.Add(uxmlElements);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/IT4080/Editor/LogViewer.uss");
            //VisualElement labelWithStyle = new Label("Hello World! With Style");
            //labelWithStyle.styleSheets.Add(styleSheet);
            //root.Add(labelWithStyle);

        }


        public void CreateGUI()
        {
            Debug.Log("Creating GUI");
            //OutOfTheBoxGUI();

            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/IT4080/Editor/LogViewer.uxml");
            VisualElement uxmlElements = visualTree.Instantiate();
            root.Add(uxmlElements);
            
            txtLog1 = rootVisualElement.Query<Label>("txtLog1").First();
            txtLog2 = rootVisualElement.Query<Label>("txtLog2").First();

            txtLog1.text = FileToText("/Users/profbutch/temp/unity_builds/TheBuild_1.log");
            txtLog2.text = FileToText("/Users/profbutch/temp/unity_builds/TheBuild_2.log");
        }





        private string FileToText(string path) {
            StreamReader reader = new StreamReader(path);
            string toReturn = reader.ReadToEnd();
            reader.Close();
            return toReturn;
        }

    }
}
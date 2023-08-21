using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

namespace It4080
{
    public class LogViewer : EditorWindow
    {

        private Label log1Title;
        private Label log2Title;

        private Label txtLog1;
        private Label txtLog2;

        public string basePath;

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

            log1Title = rootVisualElement.Query<Label>("Log1Title").First();
            log2Title = rootVisualElement.Query<Label>("Log2Title").First();

            txtLog1 = rootVisualElement.Query<Label>("txtLog1").First();
            txtLog2 = rootVisualElement.Query<Label>("txtLog2").First();

            //LoadLogs();
        }


        public void LoadLogs()
        {
            LoadLog($"{basePath}_1.log", log1Title, txtLog1);
            LoadLog($"{basePath}_2.log", log2Title, txtLog2);
        }


        private void LoadLog(string path, Label title, Label textBox)
        {
            title.text = path;
            if (File.Exists(path))
            {
                
                textBox.text = FileToText(path);
            }else
            {
                textBox.text = "File not found";
            }
        }


        private string FileToText(string path) {
            StreamReader reader = new StreamReader(path);
            string toReturn = reader.ReadToEnd();
            reader.Close();
            return toReturn;
        }

    }
}
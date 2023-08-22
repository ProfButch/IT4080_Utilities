using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

namespace It4080
{
    public class LogViewer : EditorWindow
    {
        private class LogDisplay {
            private VisualElement displayRoot;

            Label title;
            Label logText;
            

            public LogDisplay(VisualElement baseElement)
            {
                Debug.Log(baseElement);
                title = baseElement.Query<Label>("Title").First();
                logText = baseElement.Query<Label>("LogText").First();
                displayRoot = baseElement;
            }


            public void LoadLog(string path) {
                Debug.Log($"Loading log {path}");
                title.text = path;
                if (File.Exists(path)) {
                    logText.text = FileToText(path);
                } else {
                    logText.text = "File not found";
                }
                Debug.Log($"Loaded log {path}");
            }


            private string FileToText(string path) {
                StreamReader reader = new StreamReader(path);
                string toReturn = reader.ReadToEnd();
                reader.Close();
                return toReturn;
            }
        }





        private LogDisplay disp1;
        private LogDisplay disp2;

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

            disp1 = new LogDisplay(root.Query<VisualElement>("LogDisplay1").First());
            Debug.Log("Disp 1 created");
            disp2 = new LogDisplay(root.Query<VisualElement>("LogDisplay2").First());
            Debug.Log("Disp 2 created");
        }


        public void LoadLogs()
        {
            disp1.LoadLog($"{basePath}_1.log");
            disp2.LoadLog($"{basePath}_2.log");
        }


        //private void LoadLog(string path, Label title, Label textBox)
        //{
        //    title.text = path;
        //    if (File.Exists(path))
        //    {
                
        //        textBox.text = FileToText(path);
        //    }else
        //    {
        //        textBox.text = "File not found";
        //    }
        //}


        //private string FileToText(string path) {
        //    StreamReader reader = new StreamReader(path);
        //    string toReturn = reader.ReadToEnd();
        //    reader.Close();
        //    return toReturn;
        //}

    }
}
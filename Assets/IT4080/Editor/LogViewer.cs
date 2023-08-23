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

            public Label title;
            public Label logText;
            

            public LogDisplay(VisualElement baseElement)
            {                              
                title = baseElement.Query<Label>("Title").First();
                logText = baseElement.Query<Label>("LogText").First();
                displayRoot = baseElement;
                Debug.Log(baseElement);
                Debug.Log(logText);
                Debug.Log(title);
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
        private LogDisplay disp3;
        private LogDisplay disp4;

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


        private void SetupControls()
        {
            VisualElement split_1 = rootVisualElement.Query<VisualElement>("TwoLogs1").First();
            VisualElement split_2 = rootVisualElement.Query<VisualElement>("TwoLogs2").First();
            disp1 = new LogDisplay(split_1.Query<VisualElement>("LeftLog").First());
            disp2 = new LogDisplay(split_1.Query<VisualElement>("RightLog").First());
            disp3 = new LogDisplay(split_2.Query<VisualElement>("LeftLog").First());
            disp4 = new LogDisplay(split_2.Query<VisualElement>("RightLog").First());

            disp1.title.text = "Hello World!!!";
            disp1.logText.text = "Look\nHere\nYou\nShithead!!!";

            disp2.title.text = "Hello World!!!";
            disp2.logText.text = "Look\nHere\nYou\nShithead too!!!";
        }


        public void CreateGUI()
        {
            Debug.Log("Creating GUI");
            //OutOfTheBoxGUI();

            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/IT4080/Editor/LogViewer.uxml");
            VisualElement uxmlElements = visualTree.Instantiate();
            root.Add(uxmlElements);

            SetupControls();
        }


        public void LoadLogs()
        {
            disp1.LoadLog($"{basePath}_1.log");
            disp2.LoadLog($"{basePath}_2.log");
            disp3.LoadLog($"{basePath}_3.log");
            disp4.LoadLog($"{basePath}_4.log");
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
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
            public VisualElement displayRoot;
            public Label title;
            public Label logText;
            

            public LogDisplay(VisualElement baseElement)
            {                              
                title = baseElement.Query<Label>("Title").First();
                logText = baseElement.Query<Label>("LogText").First();
                displayRoot = baseElement;

                //Debug.Log("LogDisplay ------------------");
                //Debug.Log(baseElement);
                //Debug.Log(logText);
                //Debug.Log(title);
                //Debug.Log("------------------ LogDisplay");
            }


            private string FileToText(string path) {
                StreamReader reader = new StreamReader(path);
                string toReturn = reader.ReadToEnd();
                reader.Close();
                return toReturn;
            }


            public void LoadLog(string path) {
                //Debug.Log($"Loading log {path}");
                title.text = path;
                if (File.Exists(path)) {
                    logText.text = FileToText(path);
                } else {
                    logText.text = "File not found";
                }
                //Debug.Log($"Loaded log {path}");
            }
        }





        private TwoPaneSplitView mainSplit;
        private TwoPaneSplitView split1;
        private TwoPaneSplitView split2;
        private LogDisplay disp1;
        private LogDisplay disp2;
        private LogDisplay disp3;
        private LogDisplay disp4;

        public string basePath;


        private void SetupControls()
        {
            mainSplit = rootVisualElement.Query<TwoPaneSplitView>("FourLogs");

            split1 = rootVisualElement.Query<TwoPaneSplitView>("LogSplit1").First();
            split2 = rootVisualElement.Query<TwoPaneSplitView>("LogSplit2").First();

            disp1 = new LogDisplay(split1.Query<VisualElement>("LeftLog").First());
            disp2 = new LogDisplay(split1.Query<VisualElement>("RightLog").First());
            disp3 = new LogDisplay(split2.Query<VisualElement>("LeftLog").First());
            disp4 = new LogDisplay(split2.Query<VisualElement>("RightLog").First());

        }


        private void InitialLayout()
        {
            mainSplit.fixedPaneInitialDimension = mainSplit.resolvedStyle.height / 2.0f;
            split1.fixedPaneInitialDimension = split1.resolvedStyle.width / 2.0f;
            split2.fixedPaneInitialDimension = split2.resolvedStyle.width / 2.0f;

        }

        private bool is_first_update_call = true;
        public void Update() {
            // I could not figure out what event was the first event where all
            // the controls have been fully instanced and sized.  Calling
            // InitialLayout anywhere else always resulted in the various sizes
            // (sytle.width, resolvedStyle.width, contentRect.size.x) being NaN.
            if (is_first_update_call)
            {
                InitialLayout();
                is_first_update_call = false;
            }
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/IT4080/Editor/LogViewer.uxml");
            VisualElement uxmlElements = visualTree.Instantiate();
            root.Add(uxmlElements);

            //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/IT4080/Editor/LogViewer.uss");

            root.RegisterCallback<GeometryChangedEvent>(OnRootResized);

            SetupControls();

        }



        // ----------------------
        // Events
        // ----------------------
        private void OnRootResized(GeometryChangedEvent e)
        {
            // I hate UI Builder.  For some reason, everything looks and acts
            // just fine in the editor, but when run logsBaseElement always
            // has a height of 0 (unless hardcoded to be different).  After too
            // much fighting this is the solution.  Also, HOORAY, yet another way
            // to connect to a signal in C#.
            mainSplit.style.width = e.newRect.size.x;
            mainSplit.style.height = e.newRect.size.y;
        }


        public void LoadLogs()
        {
            disp1.LoadLog($"{basePath}_1.log");
            disp2.LoadLog($"{basePath}_2.log");
            disp3.LoadLog($"{basePath}_3.log");
            disp4.LoadLog($"{basePath}_4.log");
        }
    }
}
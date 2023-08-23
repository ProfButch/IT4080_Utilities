using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using System;
using Unity.VisualScripting;
using UnityEditor.Graphs;

namespace It4080
{
    public class LogViewer : EditorWindow
    {
        /**
         * 
         */
        private class LogDisplay {
            public VisualElement root;
            public Label title;
            public Label logText;
            public ToolbarButton btnMaximize;

            private Scroller vertScroll;
            private Scroller horizScroll;

            private DateTime lastFileChangeTime;
            private string logPath = string.Empty;

            public LogDisplay(VisualElement baseElement) {
                root = baseElement;
                title = root.Query<Label>("Title").First();
                logText = root.Query<Label>("LogText").First();                
                vertScroll = root.Query<ScrollView>().First().verticalScroller;
                horizScroll = root.Query<ScrollView>().First().horizontalScroller;
                btnMaximize = root.Query<ToolbarButton>("Maximize").First();
            }


            private string FileToText(string path) {
                StreamReader reader = new StreamReader(path);
                string toReturn = reader.ReadToEnd();
                reader.Close();
                return toReturn;
            }


            public void LoadLog(string path) {
                logPath = path;
                lastFileChangeTime = File.GetLastWriteTimeUtc(path);
                string timeDisplay = lastFileChangeTime.ToLocalTime().ToString();

                title.text = $"{Path.GetFileName(path)} ({timeDisplay})";
                if (File.Exists(path)) {
                    logText.text = FileToText(path);
                } else {
                    logText.text = "File not found";
                }
                ScrollToBottom();
            }


            public void ScrollToBottom()
            {
                vertScroll.value = vertScroll.highValue;
                horizScroll.value = 0;
            }


            public void ScrollToTop()
            {
                vertScroll.value = 0;
                horizScroll.value = 0;
            }

            public bool LogFileHasChanged()
            {
                bool toReturn = false;
                if (logPath != string.Empty && File.Exists(logPath))
                {
                    if(lastFileChangeTime == null)
                    {
                        toReturn = true;
                    } else
                    {
                        toReturn = File.GetLastWriteTimeUtc(logPath) != lastFileChangeTime;
                    }
                }
                return toReturn;
            }


            public void RefreshLog()
            {
                if (LogFileHasChanged())
                {
                    LoadLog(logPath);
                    ScrollToBottom();
                }
            }
        }


        /**
         * 
         */
        private class LogSplit
        {
            public TwoPaneSplitView root;
            public LogDisplay leftLog;
            public LogDisplay rightLog;


            public LogSplit(TwoPaneSplitView baseElement)
            {
                root = baseElement;
                leftLog = new LogDisplay(root.Query<VisualElement>("LeftLog").First());
                rightLog = new LogDisplay(root.Query<VisualElement>("RightLog").First());
            }


            public void showLog(LogDisplay which, bool should)
            {
                root.UnCollapse();
                which.root.visible = should;

                if (!leftLog.root.visible)
                {
                    root.CollapseChild(0);
                }

                if (!rightLog.root.visible)
                {
                    root.CollapseChild(1);
                }
            }

            public void showOnlyLog(LogDisplay which)
            {
                LogDisplay other = leftLog;
                if(which == leftLog)
                {
                    other = rightLog;
                }

                showLog(which, true);
                showLog(other, false);
            }

            public bool AreAllLogsHidden()
            {
                return !leftLog.root.visible && !rightLog.root.visible;
            }
        }




        // ---------------------------------------------------------------------
        // ---------------------------------------------------------------------

        private TwoPaneSplitView mainSplit;
        private LogSplit topSplit;
        private LogSplit botSplit;
        private ToolbarButton btnRefresh;
        private Label lblInfo;
        private ToolbarToggle[] showLogButtons = new ToolbarToggle[4];
        private ToolbarToggle tglAutoRefresh;

        private bool autoRefresh = false;
        private float refreshInterval = 1.0f;
        private float timeSinceLastCheck = 0.0f;
        

        public string basePath;


        public void CreateGUI() {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/IT4080/Editor/LogViewer.uxml");
            VisualElement uxmlElements = visualTree.Instantiate();
            root.Add(uxmlElements);

            //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/IT4080/Editor/LogViewer.uss");

            root.RegisterCallback<GeometryChangedEvent>(OnRootResized);

            SetupControls();
        }


        private void InitialLayout() {
            mainSplit.fixedPaneInitialDimension = mainSplit.resolvedStyle.height / 2.0f;
            topSplit.root.fixedPaneInitialDimension = topSplit.root.resolvedStyle.width / 2.0f;
            botSplit.root.fixedPaneInitialDimension = botSplit.root.resolvedStyle.width / 2.0f;
        }


        private bool _is_first_update_call = true;
        private bool _should_scroll_to_bottom = false;
        public void Update() {
            // I could not figure out what event was the first event where all
            // the controls have been fully instanced and sized.  Calling
            // InitialLayout anywhere else always resulted in the various sizes
            // (sytle.width, resolvedStyle.width, contentRect.size.x) being NaN.
            if (_is_first_update_call) {
                InitialLayout();
                _is_first_update_call = false;
            }

            if (_should_scroll_to_bottom)
            {
                topSplit.leftLog.ScrollToBottom();
                topSplit.rightLog.ScrollToBottom();
                _should_scroll_to_bottom = false;
            }

            HandleAutoRefresh();
        }


        // ----------------------
        // Private
        // ----------------------
        private void HandleAutoRefresh()
        {
            if (!autoRefresh) {
                return;
            }

            timeSinceLastCheck += Time.deltaTime;
            if(timeSinceLastCheck >= refreshInterval)
            {
                RefreshLogs();
                timeSinceLastCheck = 0.0f;
            }
        }


        private void SetupLogToggle(string toggleName, LogSplit split, LogDisplay disp)
        {
            ToolbarToggle logToggle = rootVisualElement.Query<ToolbarToggle>(toggleName).First();
            logToggle.RegisterValueChangedCallback((changeEvent) => OnLogToggleToggled(split, disp, changeEvent));
        }


        private void SetupMaximizeButton(LogDisplay disp, int showLogButtonIndex) {
            disp.btnMaximize.clicked += () => OnMaximizeButtonClicked(disp, showLogButtons[showLogButtonIndex]);
        }


        private void SetupControls()
        {
            mainSplit = rootVisualElement.Query<TwoPaneSplitView>("FourLogs");

            topSplit = new LogSplit(rootVisualElement.Query<TwoPaneSplitView>("LogSplit1").First());
            botSplit = new LogSplit(rootVisualElement.Query<TwoPaneSplitView>("LogSplit2").First());

            SetupLogToggle("ShowLog1", topSplit, topSplit.leftLog);
            SetupLogToggle("ShowLog2", topSplit, topSplit.rightLog);
            SetupLogToggle("ShowLog3", botSplit, botSplit.leftLog);
            SetupLogToggle("ShowLog4", botSplit, botSplit.rightLog);

            btnRefresh = rootVisualElement.Query<ToolbarButton>("Refresh");
            btnRefresh.clicked += OnRefreshPressed;

            tglAutoRefresh = rootVisualElement.Query<ToolbarToggle>("AutoRefresh").First();
            tglAutoRefresh.value = autoRefresh;
            btnRefresh.SetEnabled(!tglAutoRefresh.value);
            tglAutoRefresh.RegisterValueChangedCallback(OnAutoRefreshToggled);

            lblInfo = rootVisualElement.Query<Label>("Info").First();

            showLogButtons[0] = rootVisualElement.Query<ToolbarToggle>("ShowLog1").First();
            showLogButtons[1] = rootVisualElement.Query<ToolbarToggle>("ShowLog2").First();
            showLogButtons[2] = rootVisualElement.Query<ToolbarToggle>("ShowLog3").First();
            showLogButtons[3] = rootVisualElement.Query<ToolbarToggle>("ShowLog4").First();

            SetupMaximizeButton(topSplit.leftLog, 0);
            SetupMaximizeButton(topSplit.rightLog, 1);
            SetupMaximizeButton(botSplit.leftLog, 2);
            SetupMaximizeButton(botSplit.rightLog, 3);
        }


        private void ShowSingleLog(LogDisplay whichLog, ToolbarToggle whichToggle)
        {
            foreach (ToolbarToggle b in showLogButtons){
                b.value = b == whichToggle;
            }

            mainSplit.UnCollapse();
            LogSplit parentSplit;
            if(whichLog == topSplit.leftLog || whichLog == topSplit.rightLog)
            {
                mainSplit.CollapseChild(1);
                parentSplit = topSplit;
            }
            else
            {
                mainSplit.CollapseChild(0);
                parentSplit = botSplit;
            }

            parentSplit.showOnlyLog(whichLog);
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

        private void OnMaximizeButtonClicked(LogDisplay disp, ToolbarToggle button) {
            ShowSingleLog(disp, button);
        }


        private void OnLogToggleToggled(LogSplit split, LogDisplay disp, ChangeEvent<bool> changeEvent) {
            split.showLog(disp, changeEvent.newValue);
            mainSplit.UnCollapse();

            if (topSplit.AreAllLogsHidden()) {
                mainSplit.CollapseChild(0);
            }

            if (botSplit.AreAllLogsHidden()) {
                mainSplit.CollapseChild(1);
            }
        }


        private void OnRefreshPressed()
        {
            RefreshLogs();
            btnRefresh.Focus();
        }


        private void OnAutoRefreshToggled(ChangeEvent<bool> changeEvent)
        {
            btnRefresh.SetEnabled(!changeEvent.newValue);
            autoRefresh = changeEvent.newValue;
        }


        // ----------------------
        // Public
        // ----------------------
        public void LoadLogs()
        {
            lblInfo.text = basePath;
            topSplit.leftLog.LoadLog($"{basePath}_1.log");
            topSplit.rightLog.LoadLog($"{basePath}_2.log");
            botSplit.leftLog.LoadLog($"{basePath}_3.log");
            botSplit.rightLog.LoadLog($"{basePath}_4.log");
            _should_scroll_to_bottom = true;
        }

        public void RefreshLogs()
        {
            topSplit.leftLog.RefreshLog();
            topSplit.rightLog.RefreshLog();
            botSplit.leftLog.RefreshLog();
            botSplit.rightLog.RefreshLog();
        }
    }
}
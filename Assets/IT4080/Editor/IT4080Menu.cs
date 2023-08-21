using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.SceneManagement;


[InitializeOnLoadAttribute]
public static class IT4080Menu
{
    public const string VERSION = "0.1.0";

    [MenuItem("IT4080/About")]
    private static void MnuAbout()
    {
        Debug.Log($"IT4080 Menu Version {VERSION}");
    }

    [MenuItem("IT4080/Window")]
    private static void MnuWindow()
    {
        //It4080.It4080TheWindow window = new It4080.It4080TheWindow()
        var window = EditorWindow.GetWindow<It4080.LogViewer>(utility: true, title: "IT4080 The Flame Thrower", focus: true);
        //var window = EditorWindow.GetWindow<It4080.It4080TheWindow>(utility: true, title: "IT4080 The Flame Thrower", focus: true);
        window.ShowUtility();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PathTester : MonoBehaviour
{
    private void Start()
    {
        var tm = GetComponent<TextMeshProUGUI>();
        if (tm == null)
            return;
        tm.text = "";

        /*
        var cur = Directory.GetCurrentDirectory();
        tm.text += $"CWD: {cur}\n";
        */

        var appData = Application.dataPath;
        tm.text += $"App Data: {appData}\n";

        /*
        var appPersist = Application.persistentDataPath;
        tm.text += $"Persistent Data: {appPersist}\n";

        var appStream = Application.streamingAssetsPath;
        tm.text += $"Streaming Assets: {appStream}\n";
        */
    }
}

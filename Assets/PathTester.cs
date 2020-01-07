/*
Copyright 2020 Joshua McLean

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PathTester : MonoBehaviour
{
    [SerializeField] private bool m_showCurrentDirectory = true;
    [SerializeField] private bool m_showDataPath = true;
    [SerializeField] private bool m_showPersistentDataPath = true;
    [SerializeField] private bool m_showStreamingAssetsPath = true;

    private void Start()
    {
        var tm = GetComponent<TextMeshProUGUI>();
        if (tm == null)
            return;
        tm.text = "";

        if (m_showCurrentDirectory) {
            var cur = Directory.GetCurrentDirectory();
            tm.text += $"CWD: {cur}\n";
        }

        if (m_showDataPath) {
            var appData = Application.dataPath;
            tm.text += $"App Data: {appData}\n";
        }

        if (m_showPersistentDataPath) {
            var appPersist = Application.persistentDataPath;
            tm.text += $"Persistent Data: {appPersist}\n";
        }

        if (m_showStreamingAssetsPath) {
            var appStream = Application.streamingAssetsPath;
            tm.text += $"Streaming Assets: {appStream}\n";
        }
    }
}

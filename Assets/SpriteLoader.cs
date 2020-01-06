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

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sprite Loader
// a demo script for the 8 Bits to Infinity game jam, Shape Jam II: Art Edition
// by Joshua McLean

// to run the demo in main.unity:
// (1) create an image file C:\tmp\blue.png

// (1) attach this component to a Game Object which has a Sprite Renderer or Image component
// (2) set the file path to an absolute path
// (3) include in your instructions what the paths are for various assets in the game

// you can expand this to:
// - guarantee cross-platform support by using file system stuff
// - read a text file that allows the user to specify paths for files 
// - set up paths relative to the executable's folder
// - load images for components other than Sprite Renderer and Image
// - use the same general principles for other types of assets

public class SpriteLoader : MonoBehaviour
{
    [SerializeField, Tooltip("Absolute path to the asset")]
    string m_filePath = "";

    [SerializeField, Tooltip("Whether to clear the color (to white) when loading an image")]
    private bool m_clearColorAfterLoad = true;

    private SpriteRenderer m_spriteRenderer = null;
    private Image m_image = null;

    // verify that we have either a Sprite Renderer or Image; otherwise this component is useless
    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_spriteRenderer == null) {
            m_image = GetComponent<Image>();
            if (m_image == null) {
                Debug.LogError($"Sprite Loader requires a Sprite Renderer or an Image, but neither exists in {name}.");
                Destroy(this);
            }
        }
    }

    // apply the texture from file to the Sprite Renderer or Image
    private void Start()
    {
        // get texture from file
        var tex = LoadTexture();
        if (tex == null)
            return;

        // create rectangle for bounds
        var rect = new Rect(0, 0, tex.width, tex.height);

        // construct a Sprite for Unity to use
        var sprite = Sprite.Create(tex, rect, Vector2.one * 0.5f);

        // if we're doing a Sprite Renderer...
        if (m_spriteRenderer != null) {
            if (m_clearColorAfterLoad)
                m_spriteRenderer.color = Color.white;
            m_spriteRenderer.sprite = sprite;
        }

        // if we're doing an image...
        if (m_spriteRenderer != null) {
            if (m_image != null) {
                if (m_clearColorAfterLoad)
                    m_image.color = Color.white;
                m_image.sprite = sprite;
            }
        }
    }

    // load the texture from file
    private Texture2D LoadTexture()
    {
        // double-check in case file got removed
        if (File.Exists(m_filePath) == false) {
            Debug.LogError($"Failed to load file for {name} at [{m_filePath}].");
            return null;
        }

        // get the file data
        var data = File.ReadAllBytes(m_filePath);

        // put it in a texture
        var tex = new Texture2D(1, 1);
        tex.LoadImage(data);

        return tex;
    }
}


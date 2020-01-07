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
// (1) build the demo
// (2) add files sprite.png and ui.png to the PROJECT_Data folder
//     (where PROJECT is the name of the build)

// (1) attach this to a Game Object with Sprite Renderer or Image 
// (2) set the file path relative to the game's data directory
// (3) include instructions for the end user to include the necessary files in
//     the data directory (this will be the PROJECT_Data folder where PROJECT is
//     your project name in your built game)

// you can expand this to:
// - use a global subdirectory in the data folder to keep assets organized
// - allow multiple extensions; this code will work for any image format the
//   Unity recognizes, including jpg and png
// - load images for components other than Sprite Renderer and Image
// - use the same general principles for other types of assets
// - have a text settings file for options in this component
// - read a text file that allows the user to specify paths for files (this
//   will be difficult as they're set up to be relative to the project's install
//   directory)

public class SpriteLoader : MonoBehaviour
{
    enum ScaleMode
    {
        ScaleToLoadedImage,
        ScaleToSprite
    }

    [SerializeField,
        Tooltip("Loaded Image: Keep image the same size\n"
        + "Sprite: Scale to match the size set in the editor")]
    private ScaleMode m_scaleMode = ScaleMode.ScaleToLoadedImage;

    [SerializeField, Tooltip("Path to asset relative to build's PROJECT_Data/ folder")]
    private string m_relativeFilePath = "";

    [SerializeField,
        Tooltip("Whether to throw an error if we can't find the file\n"
        + "(disable this for Shape Jam II week 1 - you won't have image files)")]
    private bool m_missingFileIsAnError = true;

    [SerializeField, Tooltip("Whether to clear the color (to white) when loading an image")]
    private bool m_clearColorAfterLoad = true;

    private SpriteRenderer m_spriteRenderer = null;
    private Image m_image = null;

    // verify that we have either a Sprite Renderer or Image; otherwise this component is useless
    private void Awake() {
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
    private void Start() {
        if (m_spriteRenderer == null && m_image == null)
            return;

        // get texture from file
        var tex = LoadTexture();
        if (tex == null)
            return;

        // create rectangle for bounds
        var rect = new Rect(0, 0, tex.width, tex.height);

        // construct a Sprite for Unity to use
        // NOTE: this sprite defaults to 100 ppu
        var sprite = Sprite.Create(tex, rect, Vector2.one * 0.5f);

        // if we're doing a Sprite Renderer...
        if (m_spriteRenderer != null) {
            if (m_clearColorAfterLoad)
                m_spriteRenderer.color = Color.white;
            m_spriteRenderer.sprite = sprite;

            // scale if desired (sprites default to image size)
            if (m_scaleMode == ScaleMode.ScaleToSprite)
                transform.localScale = Vector2.one * 100f / sprite.rect.size;
        }

        // if we're doing an image...
        if (m_image != null) {
            if (m_clearColorAfterLoad)
                m_image.color = Color.white;
            m_image.sprite = sprite;

            // scale if desired (images default to in-editor size)
            if (m_scaleMode == ScaleMode.ScaleToLoadedImage) {
                m_image.rectTransform.sizeDelta *= sprite.rect.size / 100f;
            }
        }
    }

    private string FilePath {
        get {
            var appDataPath = Application.dataPath;
            return $"{appDataPath}/{m_relativeFilePath}";
        }
    }

    // load the texture from file
    private Texture2D LoadTexture() {
        // check for file 
        if (File.Exists(FilePath) == false) {
            if (m_missingFileIsAnError)
                Debug.LogError($"File for {name} does not exist: [{FilePath}].");

            return null;
        }

        // get the file data
        var data = File.ReadAllBytes(FilePath);

        // put it in a texture
        var tex = new Texture2D(2, 2);
        var wasLoaded = tex.LoadImage(data);

        if (wasLoaded == false) {
            Debug.LogError($"Failed to load data for {name} at [{FilePath}] (but file exists)");
            return null;
        }

        return tex;
    }
}


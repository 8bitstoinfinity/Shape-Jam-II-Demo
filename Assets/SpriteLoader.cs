using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLoader : MonoBehaviour
{
    [SerializeField] string m_filePath = "";
    [SerializeField] private bool m_clearColorAfterLoad = true;

    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_clearColorAfterLoad)
            spriteRenderer.color = Color.white;

        var tex = LoadTexture();
        if (tex == null)
            return;

        var rect = new Rect(0, 0, tex.width, tex.height);
        spriteRenderer.sprite = Sprite.Create(tex, rect, Vector2.one * 0.5f);
    }

    private Texture2D LoadTexture()
    {
        if (File.Exists(m_filePath) == false) {
            Debug.LogError($"Failed to load file for {name} at [{m_filePath}].");
            return null;
        }

        var data = File.ReadAllBytes(m_filePath);
        var tex = new Texture2D(1, 1);
        tex.LoadImage(data);
        return tex;
    }
}

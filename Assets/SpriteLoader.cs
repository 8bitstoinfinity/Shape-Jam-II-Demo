using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLoader : MonoBehaviour
{
    [SerializeField] string m_filePath = "";
    [SerializeField] private bool m_clearColorAfterLoad = true;

    private SpriteRenderer m_spriteRenderer = null;
    private Image m_image = null;

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

    private void Start()
    {
        var tex = LoadTexture();
        if (tex == null)
            return;
        var rect = new Rect(0, 0, tex.width, tex.height);
        var sprite = Sprite.Create(tex, rect, Vector2.one * 0.5f);

        if (m_spriteRenderer != null) {
            if (m_clearColorAfterLoad)
                m_spriteRenderer.color = Color.white;
            m_spriteRenderer.sprite = sprite;
        }

        if (m_image != null) {
            if (m_clearColorAfterLoad)
                m_image.color = Color.white;
            m_image.sprite = sprite;
        }
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

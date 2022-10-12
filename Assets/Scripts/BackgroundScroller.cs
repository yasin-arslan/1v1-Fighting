using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    /// <summary>
    ///Ana menüde parallax efektini elde etmek için kullanılan basit bir script.
    ///</summary>
    [Range(-1f, 1f)]
    public float backgroundSpeed;
    public Renderer backgroundRenderer;
    void Update()
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(backgroundSpeed * Time.deltaTime, 0f);
    }
}

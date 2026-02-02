using System.Collections;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    private SpriteRenderer sr;
    private float fadeSpeed = 2.5f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color c = sr.color;
        c.a -= fadeSpeed * Time.deltaTime;
        sr.color = c;

        if (c.a <= 0)
            Destroy(gameObject);
    }

    public void Init(Sprite sprite, SpriteRenderer spriteRenderer, Color color, Vector3 scale)
    {
        sr.sprite = sprite;
        sr.flipX = spriteRenderer.flipX;
        sr.color = color;
        transform.localScale = scale;
    }
}
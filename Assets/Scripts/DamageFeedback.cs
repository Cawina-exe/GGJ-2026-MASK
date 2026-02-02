using System.Collections;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    [SerializeField] private float flashTime = 0.3f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void PlayFeedback()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        sr.color = originalColor;
    }
}
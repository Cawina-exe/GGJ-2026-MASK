using System.Collections;
using UnityEngine;

public class Mask : MonoBehaviour
{
    [SerializeField] private Sprite happyMaskSprite;
    [SerializeField] private Sprite sadMaskSprite;
    [SerializeField] private bool startHappy = true;
    [SerializeField] private bool startSad = true;

    private SpriteRenderer spriteRenderer;

    private bool coowldown = false;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (startSad)
        {
            SetSadMask();
        }

        if (startHappy)
        {
            SetHappyMask();
        }
    }

    private void Update()
    {
        if (coowldown == true)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else 
        { 
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void SetHappyMask()
    {
        spriteRenderer.sprite = happyMaskSprite;
    }
    public void SetSadMask()
    {
        spriteRenderer.sprite = sadMaskSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (startSad == true)
            {
                Player player = collision.GetComponent<Player>();
                if (player.coowldownMask == false)
                {
                    player.CatchMaskSad();
                    StartCoroutine(ActiveCoowldown());
                }
            }
            if (startHappy == true)
            {
                Player player = collision.GetComponent<Player>();
                if (player.coowldownMask == false)
                {
                    player.CatchMaskHappy();
                    StartCoroutine(ActiveCoowldown());
                }
            }
        }
    }

    IEnumerator ActiveCoowldown()
    {
        coowldown = true;
        yield return new WaitForSeconds(30.0f);
        coowldown = false;
    }
}

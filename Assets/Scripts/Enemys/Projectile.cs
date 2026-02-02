using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 2f;

    private Vector2 direction;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(
                    damage,
                    transform.position,
                    10f
                );
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
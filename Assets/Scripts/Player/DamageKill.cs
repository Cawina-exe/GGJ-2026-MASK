using UnityEngine;

public class DamageKill : MonoBehaviour
{
    [SerializeField] private float damage = 1000f;
    [SerializeField] private float knockbackForce = 12f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            Vector3 fakeAttackerPos = other.transform.position + Vector3.down;

            dmg.TakeDamage(
                damage,
                fakeAttackerPos,
                knockbackForce
            );
        }
    }
}


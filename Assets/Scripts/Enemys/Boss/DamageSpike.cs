using System.Collections;
using UnityEngine;

public class DamageSpike : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private bool isActiveDamage = true;
    [SerializeField] private bool destroy = false;

    private void Update()
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private bool cooldown = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isActiveDamage) return;

        if (!other.CompareTag("Player")) return;

        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            if (cooldown) return;

            Vector3 fakeAttackerPos = other.transform.position + Vector3.down;

            dmg.TakeDamage(
                damage,
                fakeAttackerPos,
                knockbackForce
            );

            StartCoroutine(DamageCooldown());

        }
    }

    IEnumerator DamageCooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }
}

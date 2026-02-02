using Unity.VisualScripting;
using UnityEngine;

public class MaskBonus : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 12f;

    [SerializeField] private float Heal = 20f;

    [SerializeField] private bool isMaskHappy;
    [SerializeField] private bool isMaskSad;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (isMaskHappy == true)
        {
            other.GetComponent<Player>().Heal(Heal);

            Destroy(gameObject);
        }
        else if (isMaskSad == true)
        {

            IDamageable dmg = other.GetComponent<IDamageable>();

            if (dmg != null)
            {

                Vector3 fakeAttackerPos = other.transform.position + Vector3.down;

                dmg.TakeDamage(
                    damage,
                    fakeAttackerPos,
                    knockbackForce
                );

                Destroy(gameObject);
            }
        }

    }
}

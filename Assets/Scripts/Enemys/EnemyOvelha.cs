using System.Collections;
using UnityEngine;

public class EnemyOvelha : MonoBehaviour, IDamageable
{
    private float life = 50f;
    private float damage = 1f;
    private float speed = 2f;

    public Transform groundCheck;
    private float groundCheckDistance = 0.4f;
    public LayerMask groundLayer;

    public Transform leftLimit;
    public Transform rightLimit;

    private float detectDistance = 5f;
    private float heightTolerance = 1f;
    public LayerMask playerLayer;

    private float attackDistance = 1.1f;
    private float attackCooldown = 1f;

    private Rigidbody2D rb;
    private DamageFeedback feedback;
    private Transform player;
    private bool movingRight = true;
    private bool canAttack = true;

    private bool stunned;

    private Animator anim;

    private SpriteRenderer sr;

    private bool death = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        feedback = GetComponent<DamageFeedback>();


        sr.flipX = true;
    }

    void Update()
    {
        if (death)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (stunned) return;

        if (!canAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        DetectPlayer();

        if (player != null)
        {
            Chase();
            TryAttack();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!HasGroundAhead())
        {
            Flip();
            return;
        }

        float dir = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        if (transform.position.x <= leftLimit.position.x)
        {
            sr.flipX = true;
            movingRight = true;
        }
        else if (transform.position.x >= rightLimit.position.x)
        {
            sr.flipX = false;
            movingRight = false;
        }
    }

    void DetectPlayer()
    {
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            detectDistance,
            playerLayer
        );

        if (hit && Mathf.Abs(hit.transform.position.y - transform.position.y) < heightTolerance)
            player = hit.transform;
        else
            player = null;
    }

    void Chase()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        if (!HasGroundAhead())
            return;

        rb.linearVelocity = new Vector2(dir * speed * 1.4f, rb.linearVelocity.y);
        movingRight = dir > 0;
    }

    void TryAttack()
    {
        if (!canAttack) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position,attackDistance,playerLayer);

        if (hit)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage,transform.position, 8f);

                StartCoroutine(AttackCooldown());
            }
        }
    }

    bool HasGroundAhead()
    {
        Vector2 origin = groundCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    IEnumerator AttackCooldown()
    {
        anim.SetTrigger("IsAttacking");
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        anim.ResetTrigger("IsAttacking");
    }

    public void TakeDamage(float damage, Vector3 attackerPosition, float knockbackForce)
    {
        if (death) return;

        life -= damage;

        Vector2 knockDir = (transform.position - attackerPosition).normalized;
        knockDir.y = 0;
        knockDir.Normalize();

        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        feedback.PlayFeedback();

        StartCoroutine(Stun(0.2f));

        if (life <= 0)
        {
            death = true;
            anim.SetTrigger("IsDeath");
            StartCoroutine(Death(5f));
        }
    }

    IEnumerator Stun(float time)
    {
        stunned = true;
        yield return new WaitForSeconds(time);
        stunned = false;
    }

    IEnumerator Death(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
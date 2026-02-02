using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossEnemy : MonoBehaviour, IDamageable
{
    private float life = 400f;

    private float moveSpeed = 12.5f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private SpawSpike[] spawnSpike;
    [SerializeField] private SpawnString spawnString;
    [SerializeField] private Light2D[] maskLight;
    [SerializeField] AudioClip spikeclip;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip deathSound;

    private DamageFeedback feedback;
    private Rigidbody2D rb;
    private Animator anim;

    private bool canAttack = true;
    private bool death = false;

    private bool canUseMask = true;
    private bool maskHappy = false;
    private bool maskSad = false;

    [SerializeField] private float patrolChangeTime = 15f;
    private float teleportOffsetX = 1.5f;
    [SerializeField] private float spikeDelay = 0.5f;

    private int currentPatrolIndex;
    private float patrolTimer;
    private bool isAttacking;

    [SerializeField] private GameObject EndGame;
    [SerializeField] private Player playerRef;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feedback = GetComponent<DamageFeedback>();
    }

    void Update()
    {
        if (death)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!isAttacking)
        {
            Patrol();

            if (canAttack)
            {
                StartCoroutine(AttackRoutine());
            }
        }

        if (canUseMask)
        {
            int mask = Random.Range(0, 2);
            if (mask == 0 && !maskHappy)
            {
                maskHappy = true;
                maskLight[0].intensity = 5.25f;
                StartCoroutine(MaskCooldown(30f));
            }
            else if (mask == 1 && !maskSad)
            {
                maskSad = true;
                maskLight[1].intensity = 5.25f;
                StartCoroutine(MaskCooldown(30f));
            }
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolChangeTime)
        {
            patrolTimer = 0f;
            currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        }

        Transform target = patrolPoints[currentPatrolIndex];

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        rb.MovePosition(newPos);
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        int attack = Random.Range(0, 2);

        if (attack == 0)
            yield return StartCoroutine(AttackOne());
        else
            yield return StartCoroutine(AttackTwo());

        yield return new WaitForSeconds(5f);
        canAttack = true;

        anim.ResetTrigger("IsAttackHappy");
        anim.ResetTrigger("IsAttackSad");
    }

    private IEnumerator AttackOne()
    {
        AudioManager.instance.PlaySound(attackSound);
        isAttacking = true;
        anim.SetTrigger("IsAttackSad");

        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            Vector3 teleportPos = player.transform.position +
                new Vector3(teleportOffsetX, gameObject.transform.position.y, 0f);

            rb.linearVelocity = Vector2.zero;
            transform.position = teleportPos;
            spawnString.Spaw();
        }

        yield return new WaitForSeconds(5f);

        isAttacking = false;
        patrolTimer = 0f;
    }
    private IEnumerator AttackTwo()
    {
        AudioManager.instance.PlaySound(spikeclip);
        isAttacking = true;
        anim.SetTrigger("IsAttackHappy");

        Shuffle(spawnSpike);

        foreach (var spike in spawnSpike)
        {
            spike.Spaw();
            yield return new WaitForSeconds(spikeDelay);
        }

        isAttacking = false;
    }

    private void Shuffle(SpawSpike[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            SpawSpike temp = array[i];
            array[i] = array[rand];
            array[rand] = temp;
        }
    }

    public void TakeDamage(float damage, Vector3 attackerPosition, float knockbackForce)
    {
        if (death) return;

        Player player = FindAnyObjectByType<Player>();

        if (player.maskSad == true && maskHappy == true)
        {
            life -= damage;

            Vector2 knockDir = (transform.position - attackerPosition).normalized;
            knockDir.y = 0;
            knockDir.Normalize();

            rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

            feedback.PlayFeedback();

            if (life <= 0)
            {
                death = true;
                EndGame.SetActive(true);
                playerRef.EndLevel();
                anim.SetTrigger("IsDeath");
                StartCoroutine(Death(5f));
               
            }
        }
        else if (player.maskHappy == true && maskSad == true)
        {
            life -= damage;

            Vector2 knockDir = (transform.position - attackerPosition).normalized;
            knockDir.y = 0;
            knockDir.Normalize();

            rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

            feedback.PlayFeedback();

            if (life <= 0)
            {
                death = true;
                anim.SetTrigger("IsDeath");
                StartCoroutine(Death(5f));
               
            }
        }
        else
        {

        }

    }

    IEnumerator Death(float time)
    {
        AudioManager.instance.PlaySound(deathSound);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);

    }

    IEnumerator MaskCooldown(float time)
    {
        canUseMask = false;
        yield return new WaitForSeconds(time);
        canUseMask = true;

        maskHappy = false;
        maskSad = false;

        maskLight[0].intensity = 0f;
        maskLight[1].intensity = 0f;
    }
}

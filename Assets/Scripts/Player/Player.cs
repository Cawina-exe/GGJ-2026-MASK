using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float life = 100.0f;
    private float maxLife = 100.0f;
    private float damage = 10.0f;
    private float defense = 0f;

    private Rigidbody2D rb;
    private float speed = 5.0f;
    private float speedBonus = 0f;
    private float jumpForce = 8.5f;
    private bool facingRight = true;

    [SerializeField] private Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    public bool maskHappy;
    public bool maskSad;
    private bool hability;

    public bool coowldownMask = false;
    public bool coowldownMaskHability = false;

    public float scoreHappy = 50;
    public float scoreSad = 50;
    private float maskInterval = 1f;
    private float timerScore;

    [SerializeField] private GameObject maskSadEffect;

    [SerializeField] private GameObject VolumeSadEffect;
    [SerializeField] private GameObject VolumeHappyEffect;
    [SerializeField] private GameObject VolumeNormalEffect;

    [SerializeField] private GameObject worldHappy;
    [SerializeField] private GameObject worldSad;

    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private float spawnInterval = 0.05f;
    [SerializeField] private Color trailColor = new Color(1f, 1f, 1f, 0.6f);


    [SerializeField] AudioClip walkSoundclip;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioClip powerUp1;
    [SerializeField] AudioClip powerUp2;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioSource happyMusic;
    [SerializeField] AudioSource sadMusic;
    [SerializeField] AudioSource neutralMusic;
    [SerializeField] AudioSource bossMusic;

    private SpriteRenderer spriteRenderer;
    private float timerTrail;

    private bool coowldownAttack = false;
    private float coowldownAttackTime = 1f;
    private DamageFeedback feedback;

    private bool inKnockback;

    private Animator anim;

    private bool death = false;
    private bool isBow = false;

    [SerializeField] private Animator curtainAnim;
    private UIManager uiManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        feedback = GetComponent<DamageFeedback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        uiManager = UIManager.instance;
        AudioManager.instance.StopSound(walkSoundclip);
    }

    void Update()
    {
        if (isBow) return;
        if (death) return;

        timerTrail += Time.deltaTime;
        timerScore += Time.deltaTime;


        if (!inKnockback)
        {
            if (!coowldownAttack)
            {
                Move();
                Jump();
                Attack();
            }
        }

        HandleJumpAnimation();
        CheckGround();
        Power();
        Score();

        if (maskHappy == true)
        {
            if (timerScore >= maskInterval)
            {
                scoreHappy += 2;
                scoreSad -= 0.5f;
                timerScore = 0f;
            }
        }
        else if (maskSad == true) 
        {
            if (timerScore >= maskInterval)
            {
                scoreHappy -= 0.5f;
                scoreSad += 2;
                timerScore = 0f;
            }
        }
        else 
        {
            if (timerScore >= maskInterval)
            {
                scoreHappy -= 1;
                scoreSad -= 1;
                timerScore = 0f;
            }
        }

        if (hability == true)
        {
            if (maskSad == true)
            {
                defense = 10f;
                speed = 5f;
                coowldownAttackTime = 1f;
            }
            else if (maskHappy == true)
            {
                speed = 7.5f;
                coowldownAttackTime = 0.5f;

                if (timerTrail >= spawnInterval)
                {
                    SpawnTrail();
                    timerTrail = 0f;
                }
            }
            else
            {
                speed = 5f;
                defense = 0f;
                coowldownAttackTime = 1f;
            }
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !coowldownAttack)
        {
            StartCoroutine(AttackCooldown());
            AudioManager.instance.PlaySound(attackSound);
            
        }
    }

    void Power()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && coowldownMaskHability == false)
        {
            if (maskHappy)
            {
                AudioManager.instance.PlaySound(powerUp1);
                UIPlayer uIPlayer = FindAnyObjectByType<UIPlayer>();

                uIPlayer.CooldownHability();

                StartCoroutine(MaskDurationHability());
                
            }
            else if (maskSad)
            {
                AudioManager.instance.PlaySound(powerUp2);
                UIPlayer uIPlayer = FindAnyObjectByType<UIPlayer>();
                
                uIPlayer.CooldownHability();

                maskSadEffect.SetActive(true);
                StartCoroutine(MaskDurationHability());
                
            }
        }
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        
        float finalSpeed = moveX * speed;

        if (moveX != 0)
            finalSpeed += speedBonus * Mathf.Sign(moveX);

        rb.linearVelocity = new Vector2(finalSpeed, rb.linearVelocity.y);
        anim.SetFloat("WalkingSpeed", Mathf.Abs(rb.linearVelocity.x));

        if (moveX > 0)
            facingRight = true;
        else if (moveX < 0)
            facingRight = false;

        spriteRenderer.flipX = !facingRight;
        
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            AudioManager.instance.PlaySound(jumpSound);
        }
    }

    void HandleJumpAnimation()
    {
        if (!isGrounded && rb.linearVelocity.y > 0.1f)
        {
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsInTheSky", false);
            anim.SetBool("IsFalling", false);
        }
        else if (!isGrounded && rb.linearVelocity.y < -0.1f)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsInTheSky", true);
            anim.SetBool("IsFalling", false);
        }
        else if (isGrounded)
        {

            anim.SetBool("IsInTheSky", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

    }

    public void TakeDamage(float damage, Vector3 attackerPosition, float knockbackForce)
    {
        Debug.Log("Player took " + damage + " damage.");

        if (isBow) return;
        if (death) return;
        anim.SetBool("IsReceiveAttack", true);

        damage -= defense;
        if (damage < 0) damage = 0;

        life -= damage;


        Vector2 knockDir = (transform.position - attackerPosition).normalized;
        knockDir.y = 0;
        knockDir.Normalize();

        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        feedback.PlayFeedback();

        StartCoroutine(KnockbackRoutine(0.25f));

        if (life <= 0)
            GameOver();
        AudioManager.instance.PlaySound(hitSound);
    }

    public void Heal(float amount)
    {
        life += amount;
        if (life > maxLife)
        {
            life = maxLife;
        }
    }

    public void Score()
    {
        if (scoreHappy >= 100)
        {
            speedBonus = 2.5f;
        }
        else 
        { 
            speedBonus = 0f;
        }

        if (scoreSad >= 100)
        {
            damage = 20f;
        }
        else
        {
            damage = 10.0f;
        }

        if (scoreHappy < 0 && scoreSad < 0)
        {
            GameOver();
        }

    }

    public void GameOver()
    {
        anim.SetBool("IsDeath", true);
        rb.linearVelocity = Vector2.zero;
        enabled = false;
        death = true;
        StartCoroutine(BowTime());
        uiManager.Lose();
        AudioManager.instance.PlaySound(deathSound);
       bossMusic.Stop();
    }

    public void CatchMaskHappy()
    {
        if (happyMusic != null) 
        {
            happyMusic.Play();
        }
        if (sadMusic != null) 
        {
            sadMusic.Stop();
        }
        if (neutralMusic != null) 
        {
            neutralMusic.Stop();
        }
        curtainAnim.SetTrigger("Mask");
        maskHappy = true;
        maskSad = false;
        
        worldHappy.SetActive(true);
        worldSad.SetActive(false);

        VolumeHappyEffect.SetActive(true);
        VolumeNormalEffect.SetActive(false);
        VolumeSadEffect.SetActive(false);

        coowldownMask = true;
        StartCoroutine(MaskDuration());
    }
    public void CatchMaskSad()
    {
        if (happyMusic != null)
        {
            happyMusic.Stop();
        }
        if (sadMusic != null)
        {
            sadMusic.Play();
        }
        if (neutralMusic != null)
        {
            neutralMusic.Stop();
        }
        curtainAnim.SetTrigger("Mask");
        maskSad = true;
        maskHappy = false;

        worldHappy.SetActive(false);
        worldSad.SetActive(true);

        VolumeHappyEffect.SetActive(false);
        VolumeNormalEffect.SetActive(false);
        VolumeSadEffect.SetActive(true);

        coowldownMask = true;
        StartCoroutine(MaskDuration());
    }
    public void LoseMasks()
    {
        if (happyMusic != null)
        {
            happyMusic.Stop();
        }
        if (sadMusic != null)
        {
            sadMusic.Stop();
        }
        if (neutralMusic != null)
        {
            neutralMusic.Play();
        }

        maskHappy = false;
        maskSad = false;

        worldHappy.SetActive(true);
        worldSad.SetActive(true);

        VolumeHappyEffect.SetActive(false);
        VolumeNormalEffect.SetActive(true);
        VolumeSadEffect.SetActive(false);

        coowldownMask = false;
    }

    public void EndLevel()
    {
        isBow = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsBowing", true);
        StartCoroutine(BowTime());
        uiManager.Win();
    }
    IEnumerator BowTime()
    {
        yield return new WaitForSeconds(0.5f);
        curtainAnim.SetTrigger("Close");
    }
    IEnumerator MaskDuration()
    {
        yield return new WaitForSeconds(30.0f); 
        LoseMasks();
    }

    IEnumerator MaskDurationHability()
    {
        hability = true;
        coowldownMaskHability = true;
        yield return new WaitForSeconds(5f);
        maskSadEffect.SetActive(false);
        hability = false;
        yield return new WaitForSeconds(5f);
        coowldownMaskHability = false;
    }

    IEnumerator AttackCooldown()
    {
        rb.linearVelocity = Vector2.zero;
        coowldownAttack = true;
        anim.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(coowldownAttackTime / 2);

        Vector2 attackPos = transform.position + (facingRight ? Vector3.right : Vector3.left);

        Collider2D hit = Physics2D.OverlapCircle(
            attackPos,
            1.2f,
            LayerMask.GetMask("Enemy")
        );

        if (hit)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(
                    damage,
                    transform.position,
                    10f
                );
            }
        }

        yield return new WaitForSeconds(coowldownAttackTime / 2);

        coowldownAttack = false;
        anim.SetBool("IsAttacking", false);
    }

    IEnumerator KnockbackRoutine(float time)
    {
        inKnockback = true;
        yield return new WaitForSeconds(time);
        inKnockback = false;
        anim.SetBool("IsReceiveAttack", false);
    }

    void SpawnTrail()
    {
        GameObject trail = Instantiate(
            trailPrefab,
            transform.position,
            transform.rotation
        );

        PlayerShadow t = trail.GetComponent<PlayerShadow>();
        t.Init(spriteRenderer.sprite,spriteRenderer , trailColor, transform.localScale);
    }
}

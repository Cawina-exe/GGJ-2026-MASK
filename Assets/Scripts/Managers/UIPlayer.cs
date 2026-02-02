using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Image playerHp;
    [SerializeField] private Image maskSad;
    [SerializeField] private Image maskHappy;

    int hpMax = 100;
    int maskSadMax = 100;
    int maskHappyMax = 100;

    Player player;

    [SerializeField] private Image hability;

    private float timerhability = 0f;
    private bool habilityOnCooldown = false;
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        playerHp.fillAmount = (float)player.life / hpMax;
        maskSad.fillAmount = (float)player.scoreSad / maskSadMax;
        maskHappy.fillAmount = (float)player.scoreHappy / maskHappyMax;

        if (habilityOnCooldown)
        {
            timerhability += Time.deltaTime;
            hability.fillAmount = timerhability / 10f;
            if (timerhability >= 10f)
            {
                habilityOnCooldown = false;
                timerhability = 0f;
                hability.fillAmount = 1f; 
            }
        }
    }

    public void CooldownHability()
    {
        habilityOnCooldown = true;
    }
}

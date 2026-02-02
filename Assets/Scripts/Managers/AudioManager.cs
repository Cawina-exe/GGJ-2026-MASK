using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class AudioManager : MonoBehaviour
{
    #region Declarations
    public static AudioManager instance;
    [Header("Music")]
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource happyMusic;
    [SerializeField] private AudioSource sadMusic;
    [SerializeField] private AudioSource bossMusic;
    [Header("Sound Effects")]
    [SerializeField] private AudioSource applause;
    [SerializeField] private AudioSource curtainOpen;
    [SerializeField] private AudioSource curtainClose;
    [SerializeField] private AudioSource uiButton;
    [SerializeField] private AudioSource hitSound;
    [Header("Player Sounds")]
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource powerUp1;    //speed boost (happy)
    [SerializeField] private AudioSource powerUp2;    //shield (sad)
    [Header("Enemy Sounds")]
    [SerializeField] private AudioSource enemyWalk;
    [SerializeField] private AudioSource enemyAttackSheep;
    [SerializeField] private AudioSource enemyAttackWolf;
    [SerializeField] private AudioSource enemyDeath;
    [Header("Boss Sounds")]
    [SerializeField] private AudioSource bossDash;
    [SerializeField] private AudioSource bossAttack1;
    [SerializeField] private AudioSource bossAttack2;
    [SerializeField] private AudioSource bossDeath;


    [SerializeField] private AudioSource audioPrefab;
   
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region SoundActivation
    public void PlaySound(AudioClip clip)
    {
        AudioSource source = Instantiate(audioPrefab);
        source.clip = clip;
        source.Play();
        //sound = mainMenuMusic;
        //sound.Play();
    }
    public void StopSound(AudioClip clip)
    {
        AudioSource source = Instantiate(audioPrefab);
        source.clip = clip;
        source.Stop();
        //sound = mainMenuMusic;
        //sound.Play();
    }


    #endregion
}

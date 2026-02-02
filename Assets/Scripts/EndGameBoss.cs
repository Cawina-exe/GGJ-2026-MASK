using UnityEngine;

public class EndGameBoss : MonoBehaviour
{
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject MapCombate;
    [SerializeField] private GameObject MapNormal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Boss.SetActive(true);
        MapCombate.SetActive(true);
        MapNormal.SetActive(false);
    }

}

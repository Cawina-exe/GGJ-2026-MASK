using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDeFase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<Player>().EndLevel();
    }
}

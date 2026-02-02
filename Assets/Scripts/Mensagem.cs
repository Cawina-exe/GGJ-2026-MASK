using UnityEngine;

public class Mensagem : MonoBehaviour
{
    [SerializeField] private GameObject mensagemUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mensagemUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mensagemUI.SetActive(false);
        }
    }
}

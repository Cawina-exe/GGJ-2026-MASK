using UnityEngine;

public class SpawnString : MonoBehaviour
{
    [SerializeField] private GameObject stringPrefab;
    public void Spaw()
    {
        Instantiate(stringPrefab, gameObject.transform.position, Quaternion.identity);
    }
}

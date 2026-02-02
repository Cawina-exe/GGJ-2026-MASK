using UnityEngine;

public class SpawSpike : MonoBehaviour
{
    [SerializeField] private GameObject spikePrefab;
    public void Spaw()
    {
        Instantiate(spikePrefab, gameObject.transform.position, Quaternion.identity);
    }
}

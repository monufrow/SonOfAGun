using UnityEngine;

public class TWSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject tumbleweedPrefab;
    public float spawnInterval = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnTumbleweed", 1f, spawnInterval);
    }


    // Update is called once per frame
    void Update()
    {


    }
    void SpawnTumbleweed()
    {
        Instantiate(tumbleweedPrefab, transform.position, Quaternion.identity);
    }
}

using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {
    public GameObject[] sticks;
    public float minSpawnTime;
    public float maxSpawnTime;
    public bool spawning;
    public float spawnRadius;

    public float spawnHeight;

	// Use this for initialization
	void Start () {
        StartCoroutine(Spawner());
	}

    IEnumerator Spawner()
    {
        while(spawning)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            Vector3 randomPos = transform.position;
            randomPos.y = spawnHeight;
            randomPos.x += Random.Range(-spawnRadius, spawnRadius);
            randomPos.z += Random.Range(-spawnRadius, spawnRadius);

            Instantiate(sticks[(int)Random.Range(0, sticks.Length)], randomPos, Quaternion.identity);
        }
    }
}

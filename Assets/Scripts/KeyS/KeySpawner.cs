using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public static KeySpawner Instance;

    public GameObject keyPrefab;
    public List<Transform> keySpawnPoints;

    public static string currentKeyRoomName = "";

    void Awake()
    {
        // Make sure Instance is set up correctly
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SpawnKey();
    }

    public void SpawnKey()
    {
        if (keySpawnPoints.Count == 0 || keyPrefab == null)
        {
            Debug.LogError("Missing key prefab or spawn points!");
            return;
        }

        int index = Random.Range(0, keySpawnPoints.Count);
        Transform spawnPoint = keySpawnPoints[index];

        Instantiate(keyPrefab, spawnPoint.position, Quaternion.identity);
        currentKeyRoomName = spawnPoint.name;

        Debug.Log("Key spawned at: " + currentKeyRoomName);
    }
}

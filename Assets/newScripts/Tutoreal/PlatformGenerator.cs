using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab; // Префаб платформы
    public Transform player; // Трансформ игрока
    public float spawnDistance = 20f; // Расстояние, на котором создаются новые платформы
    public float platformLength = 10f; // Длина одной платформы
    public float removeDistance = 30f; // Расстояние, на котором платформы удаляются

    private float nextSpawnPoint = 0f; // Точка, где будет создана следующая платформа
    private List<GameObject> activePlatforms = new List<GameObject>(); // Список активных платформ

    void Update()
    {
        // Проверяем, нужно ли создать новую платформу
        if (player.position.z > nextSpawnPoint - spawnDistance)
        {
            SpawnPlatform();
        }

        // Проверяем, нужно ли удалить старые платформы
        RemoveOldPlatforms();
    }

    void SpawnPlatform()
    {
        
        // Создаем новую платформу
        GameObject newPlatform = Instantiate(platformPrefab, new Vector3(0, 0, nextSpawnPoint), platformPrefab.transform.rotation);
        activePlatforms.Add(newPlatform); // Добавляем платформу в список активных
        // Обновляем точку для следующей платформы
        nextSpawnPoint += platformLength;
    }

    void RemoveOldPlatforms()
    {
        // Удаляем платформы, которые находятся далеко позади игрока
        if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.z < player.position.z - removeDistance)
        {
            Destroy(activePlatforms[0]);
            activePlatforms.RemoveAt(0);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] GameObject _gameObjectForPrefab;
        [SerializeField, Min(5.0f)] float _respawnDelay = 10.0f;

        GameObject _spawnedGameObject;
        FlockOfEnemies _flock;
        
        void Start()
        {
            _gameObjectForPrefab.SetActive(false);
            Spawn();
        }

        void Spawn()
        {
            if (_flock != null) _flock.OnRespawnRequest -= Respawn;

            _spawnedGameObject = Instantiate(_gameObjectForPrefab, _gameObjectForPrefab.transform.parent);
            _flock = _spawnedGameObject.GetComponent<FlockOfEnemies>();

            _flock.OnRespawnRequest += Respawn;
            _spawnedGameObject.SetActive(true);
        }

        void Respawn()
        {
            StartCoroutine(RespawnWithDelay(_respawnDelay));
        }

        IEnumerator RespawnWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Spawn();
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameObject projectilePrefab;

    [Header("Gun Parameters")] 
    [SerializeField] private float projectileSpawnOffsetZ;
    [SerializeField] private float shootingReload;

    private bool _isGameActive;
    private GameStatesEventSO _gameStatesEvent;

    public void Initialize(GameStatesEventSO eventSO)
    {
        _gameStatesEvent = eventSO;
    }
    
    public void SetGameState(GameState state)
    {
        _isGameActive = state == GameState.Playing;
        StopAllCoroutines();

        if (_isGameActive)
        {
            StartCoroutine(SpawnProjectile());
        }
    }
    
    private IEnumerator SpawnProjectile()
    {
        while (_isGameActive)
        {
            yield return new WaitForSeconds(shootingReload);
            Vector3 spawnPos = transform.position + new Vector3(0, 0, projectileSpawnOffsetZ);
            
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(90,0,0));
            projectile.GetComponent<ProjectileBehaviour>().Initialize(_gameStatesEvent);
        }
    }
}

using System;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    private GameStatesEventSO _gameStateEvent;
    private WaypointPath _path;
    private FormationLayoutSO _layout;
    private float _speed;
    private int _currentWaypointIndex;
    private int _enemyCount;
    private bool _isGameActive;

    public Action<FormationController> OnFormationEmpty;

    private void OnDestroy()
    {
        if (_gameStateEvent != null)
            _gameStateEvent.OnRaised -= HandleStateChanged;
    }

    public void Initialize(WaypointPath path, float speed, FormationLayoutSO layout, WaveSO wave, GameStatesEventSO gameStatesEvent, int roundIndex)
    {
        _path = path;
        _speed = speed;
        _layout = layout;
        _gameStateEvent = gameStatesEvent;

        _gameStateEvent.OnRaised += HandleStateChanged;

        _isGameActive = true;

        SpawnSlots(wave, roundIndex);
    }

    private void HandleStateChanged(GameState state)
    {
        _isGameActive = state == GameState.Playing;

        foreach (Transform child in transform)
        {
            EnemyBehaviour behaviour = child.GetComponent<EnemyBehaviour>();
            if (behaviour != null)
                behaviour.SetGameState(state);
        }
    }
    
    private void Update()
    {
        if(_path == null || !_isGameActive) return;
        MoveToWaypoint();
    }
    
    private void MoveToWaypoint() // Move a formação em direção ao waypoint atual a uma velocidade constante
    {
        Transform targetWaypoint = _path.GetWayPoint(_currentWaypointIndex);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            _speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _path.GetWaypointCount(); // Quando chega no último waypoint, volta para o índice 0, fazendo um loop
        }
    }

    private void SpawnSlots(WaveSO wave, int roundIndex) 
    {
        Vector3[] slots = _layout.GetSlotPositions();
        int slotIndex = 0;

        foreach (SpawnEntry entry in wave.spawnEntries)
        {
            for (int i = 0; i < entry.quantity; i++)
            {
                if (slotIndex >= slots.Length) break;
                
                // instancia na posição do Formation mesmo
                GameObject enemy = Instantiate(
                    entry.enemyData.enemyPrefab,
                    transform,  // já define o pai e usa a posição do Formation
                    false        // false = não herda a posição global do pai
                );

                // posiciona no slot correto em espaço local
                enemy.transform.localPosition = slots[slotIndex];

                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;

                EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();
                if (behaviour != null)
                {
                    behaviour.Initialize(entry.enemyData, roundIndex, _gameStateEvent);
                    behaviour.SetGameState(GameState.Playing);
                    behaviour.OnEnemyDestroyed += HandleEnemyDestroyed;
                    _enemyCount++;
                }
                
                slotIndex++;
            }
        }
    }

    private void HandleEnemyDestroyed()
    {
        _enemyCount--;
        
        Debug.Log("Inimigo Destruído");

        if (_enemyCount <= 0)
        {
            Debug.Log("Todos Destruído");
            OnFormationEmpty?.Invoke(this);
        }
    }
}

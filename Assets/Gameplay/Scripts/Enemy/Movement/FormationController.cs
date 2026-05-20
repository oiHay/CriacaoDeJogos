using System;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    [SerializeField] private WaypointPath _path;
    [SerializeField] private float _speed;
    private int _currentWaypointIndex;
    [SerializeField] private FormationLayoutSO _layout;
    
    [Header("Teste temporario")]
    [SerializeField] private WaveSO testWave;

    private void Start()
    {
        transform.position = _path.GetWayPoint(0).position;
        Initialize(_path, _speed, _layout);
    }

    public void Initialize(WaypointPath path, float speed, FormationLayoutSO layout) // Recebe o caminho e a velocidade do SpawnManager
    {
        _path = path;
        _speed = speed;
        _layout = layout;

        SpawnSlots();
    }

    // private void SpawnSlots()
    // {
    //     Vector3[] slots = _layout.GetSlotPositions();
    //
    //     foreach (Vector3 slotPosition in slots)
    //     {
    //         GameObject slot = new GameObject("Slot");
    //         slot.transform.SetParent(transform);
    //         slot.transform.localPosition = slotPosition;
    //     }
    // }
    
    private void SpawnSlots() // Test de spawn
    {
        Vector3[] slots = _layout.GetSlotPositions();
        int slotIndex = 0;

        foreach (SpawnEntry entry in testWave.spawnEntries)
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
                
                slotIndex++;
            }
        }
    }

    private void Update()
    {
        if(_path == null) return;
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
}

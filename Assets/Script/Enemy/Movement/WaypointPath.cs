using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    // Código para criar visualização entre os waypoints da cena, facilitando implementação dos mesmos e permitindo visualização do caminho feito pelos inimigos presentes na wave
    
    [SerializeField] private Color pathColor = Color.yellow;
    
    public Transform GetWayPoint(int index)
    {
        return transform.GetChild(index);
    }

    public int GetWaypointCount()
    {
        return transform.childCount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = pathColor;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform current = transform.GetChild(i);
            Transform next = transform.GetChild(i + 1);
            Gizmos.DrawLine(current.position, next.position);
        }
    }
}

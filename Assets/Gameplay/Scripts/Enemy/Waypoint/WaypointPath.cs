using System;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
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

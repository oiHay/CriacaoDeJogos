using System;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private float gizmoRadius = 0.3f;
    [SerializeField] private Color gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}

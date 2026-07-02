using System;
using UnityEngine;

public class ExitZoneObjective : TutorialObjective
{
    [SerializeField] private GameObject obj;
    
    private bool _active;

    public override void StartObjective()
    {
        _active = true;
        obj.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!_active) return;
        if(!other.CompareTag("Player")) return;

        _active = false;
        obj.SetActive(false);
        Completed();
    }
}

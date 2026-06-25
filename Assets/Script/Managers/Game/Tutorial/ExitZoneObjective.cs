using System;
using UnityEngine;

public class ExitZoneObjective : TutorialObjective
{
    private bool _active;

    public override void StartObjective()
    {
        _active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!_active) return;
        if(!other.CompareTag("Player")) return;

        _active = false;
        Completed();
    }
}

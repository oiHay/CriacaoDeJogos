using System;
using UnityEngine;

public abstract class TutorialObjective : MonoBehaviour
{
    public event Action OnCompleted;

    public abstract void StartObjective();

    protected void Completed()
    {
        OnCompleted?.Invoke();
        GameManager.Instance.ChangeState(GameState.Start);
    }
}

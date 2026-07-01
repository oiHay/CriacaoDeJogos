using UnityEngine;

public class KillEnemyObjective : TutorialObjective
{
    [SerializeField] private EnemyBehaviour enemy;

    public override void StartObjective()
    {
        enemy.OnEnemyDestroyed += Completed;
    }
}

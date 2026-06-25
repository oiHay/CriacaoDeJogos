using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialObjective objective;
    [SerializeField] private PlayerSceneAnimation sceneAnimation;

    private void OnEnable() => DialogueManager.OnDialogueEnded += StartObjective;
    private void OnDisable() => DialogueManager.OnDialogueEnded -= StartObjective;

    private void StartObjective()
    {
        objective.OnCompleted += OnObjectiveCompleted;
        objective.StartObjective();
    }

    private void OnObjectiveCompleted()
    {
        objective.OnCompleted -= OnObjectiveCompleted;
        sceneAnimation.PlayExitAnimation();
    }
}

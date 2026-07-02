using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private PlayerStatsRuntimeSO playerStats;
    [SerializeField] private PowerUpSO tutorialPowerUp;
    [SerializeField] private PowerUpPanelController panelController;

    public static event Action OnPowerUpPicked;

    public static PowerUpManager Instance { get; private set; }

    private void Awake() => Instance = this;

    private void OnEnable() => gameStateEvent.OnRaised += HandleStateChanged;
    private void OnDisable() => gameStateEvent.OnRaised -= HandleStateChanged;

    private void HandleStateChanged(GameState state)
    {
        if (state != GameState.ChoosingPowerUp) return;

        if (panelController == null)
        {
            Debug.LogError("PowerUpManager: panelController is not assigned in the Inspector!", this);
            return;
        }

        panelController.Show(new[] { tutorialPowerUp });
    }

    public void OnPowerUpChosen(PowerUpSO chosen)
    {
        playerStats.Apply(chosen);

        if (OnPowerUpPicked != null)
            OnPowerUpPicked.Invoke();
        else
            CustomSceneManager.LoadNextScene();
    }
}

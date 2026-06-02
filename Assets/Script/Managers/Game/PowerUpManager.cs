using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private PlayerStatsRuntimeSO playerStats;
    [SerializeField] private PowerUpSO[] allPowerUps;
    [SerializeField] private PowerUpPanelController panelController;
    
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
        
        var options = PickRandom(3);
        panelController.Show(options);
    }

    public void OnPowerUpChosen(PowerUpSO chosen)
    {
        playerStats.Apply(chosen);
        GameManager.Instance.ChangeState(GameState.Playing);
        // Botar aqui para trocar de cena
    }

    private PowerUpSO[] PickRandom(int count)
    {
        var shuffled = (PowerUpSO[])allPowerUps.Clone();

        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        return shuffled[..count];
    }
}

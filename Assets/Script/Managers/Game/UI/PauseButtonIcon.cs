using UnityEngine;
using UnityEngine.UI;

public class PauseButtonIcon : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite pausedSprite;
    [SerializeField] private Sprite playingSprite;

    private void OnEnable()
    {
        gameStateEvent.OnRaised += HandleStateChanged;
        HandleStateChanged(gameStateEvent.gameStateAtual);
    }

    private void OnDisable()
    {
        gameStateEvent.OnRaised -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        icon.sprite = state == GameState.Paused ? pausedSprite : playingSprite;
    }
}

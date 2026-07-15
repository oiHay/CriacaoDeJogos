using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameState initialState = GameState.Playing;
    [SerializeField] private AudioClip sceneMusic;
    [SerializeField] private List<AudioClip> sceneAmbience;
    
    private void Start()
    {
        GameManager.Instance.ChangeState(initialState);
        AudioManager.Instance.PlayMusic(sceneMusic);
        AudioManager.Instance.PlayAmbience(sceneAmbience);
        
        if (initialState == GameState.EnterLevel)
            PlayerSceneAnimation.Instance.PlayEnterAnimation(() => GameManager.Instance.ChangeState(GameState.Playing));
    }
}

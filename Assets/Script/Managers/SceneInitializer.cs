using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameState initialState = GameState.Playing;
    [SerializeField] private AudioClip sceneMusic;
    [SerializeField] private List<AudioClip> sceneAmbience;
    
    private IEnumerator Start()
    {
        AudioManager.Instance.PlayMusic(sceneMusic);
        AudioManager.Instance.PlayAmbience(sceneAmbience);
        
        while (SceneTransition.IsTransitioning)
            yield return null;
        
        GameManager.Instance.ChangeState(initialState);
        
        if (initialState == GameState.EnterLevel)
            PlayerSceneAnimation.Instance.PlayEnterAnimation(() => GameManager.Instance.ChangeState(GameState.Playing));
    }
}

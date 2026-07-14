using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameState initialState = GameState.Playing;
    [SerializeField] private AudioClip sceneMusic;
    
    private void Start()
    {
        GameManager.Instance.ChangeState(initialState);
        AudioManager.Instance.PlayMusic(sceneMusic);
        
        if (initialState == GameState.EnterLevel)
            PlayerSceneAnimation.Instance.PlayEnterAnimation(() => GameManager.Instance.ChangeState(GameState.Playing));
    }
}

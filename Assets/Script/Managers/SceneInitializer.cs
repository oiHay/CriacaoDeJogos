using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameState initialState = GameState.Playing;
    
    private void Start()
    {
        GameManager.Instance.ChangeState(initialState);
        
        if (initialState == GameState.EnterLevel)
            PlayerSceneAnimation.Instance.PlayEnterAnimation(() => GameManager.Instance.ChangeState(GameState.Playing));
    }
}

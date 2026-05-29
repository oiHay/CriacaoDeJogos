using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private GameStatesEventSO _gameStatesEvent;
    private Rigidbody _rb;
    private Vector3 _velocityBeforePause;
    private Vector3 _direction = Vector3.forward;
    private float _projectileSpeed;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        if(_gameStatesEvent != null)
            _gameStatesEvent.OnRaised -= HandleStateChanged;
    }

    public void SetDirection(Vector3 direction) // Método que dita para qual direção o projétil deve se moder
    {
        _direction = direction; // Nesse caso, ele se move em direção determinada pela variável _direction
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_direction * _projectileSpeed, ForceMode.Impulse);
    }
    
    public void Initialize(GameStatesEventSO eventSO, float speed)
    {
        _gameStatesEvent = eventSO;
        _gameStatesEvent.OnRaised += HandleStateChanged;
        _projectileSpeed = speed;
    }
    
    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Paused) // impede a física de agir no objeto, se o gameState sair de play o projétil para na cena
        {
            _velocityBeforePause = _rb.linearVelocity;
            _rb.linearVelocity = Vector3.zero;
            _rb.isKinematic = true; 
        }
        else // se o gameState estiver em playing, o projétil pode se mover na cena
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = _velocityBeforePause; // restaura a velocidade anterior
        }
    }
}

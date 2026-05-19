using System;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;

    private GameStatesEventSO _gameStatesEvent;
    private Rigidbody _rb;
    private Vector3 _velocityBeforePause;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        if(_gameStatesEvent != null)
            _gameStatesEvent.OnRaised -= HandleStateChanged;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.forward * projectileSpeed, ForceMode.Impulse);
    }
    
    public void Initialize(GameStatesEventSO eventSO)
    {
        _gameStatesEvent = eventSO;
        _gameStatesEvent.OnRaised += HandleStateChanged;
    }
    
    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Paused)
        {
            _velocityBeforePause = _rb.linearVelocity;
            _rb.linearVelocity = Vector3.zero;
            _rb.isKinematic = true; // impede a física de agir no objeto
        }
        else
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = _velocityBeforePause; // restaura a velocidade anterior
        }
    }
}

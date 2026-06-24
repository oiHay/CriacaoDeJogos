using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed;

    [Header("Tilt Parameters")] 
    [SerializeField] private float tiltAmount = 15f;
    [SerializeField] private float tiltSpeed = 5f;
    
    [Header("Bounder Parameters")]
    [SerializeField] private float minBoundZ;
    [SerializeField] private float maxBoundZ;
    [SerializeField] private float boundX;

    private Rigidbody _playerRb;
    private bool _isGameActive;
    private float _verticalInput;
    private float _horizontalInput;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    public void SetGameState(GameState state)
    {
        _isGameActive = state == GameState.Playing;
    }

    private void Update()
    {
        if(!_isGameActive) return;
        
        _verticalInput = Input.GetAxis("Vertical"); // Pega o valor dos inputs verticais W(1) e S(-1)
        _horizontalInput = Input.GetAxis("Horizontal"); // Pega o valor dos inputs horizontais A(-1) e D(1)
        
        HandleTilt(_horizontalInput, _verticalInput);
        HandleBounder();
    }

    private void FixedUpdate()
    {
        if(!_isGameActive) return;
        HandleMoveInput(_horizontalInput, _verticalInput); // fazendo isso, os inputs não precisam estar dentro dos dois métodos
    }

    private void HandleMoveInput(float horizontalInput, float verticalInput)
    {
        _playerRb.AddForce(Vector3.forward * (moveSpeed * verticalInput));
        _playerRb.AddForce(Vector3.right * (moveSpeed * horizontalInput));
    }

    private void HandleTilt(float horizontalInput, float verticalInput)
    {
        float targetTiltX = -verticalInput * tiltAmount; //Variável que determina para onde o objeto player inclina e quanto inclina a partir do input vertical
        float targetTiltZ = -horizontalInput * tiltAmount; //Variável que determina para onde o objeto player inclina e quanto inclina a partir do input horizontal
        

        Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ); // determina em quais angulos o player pode inclinar
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime); // suavização entre rotação atual (0) e a atingida ao andar (tiltAmount)
    }

    private void HandleBounder()
    {
        if (!_isGameActive) return;
        
        Vector3 pos = transform.position; // a variável pos pega os valores do transform.position
        Vector3 vel = _playerRb.linearVelocity; // a variável vel pega os valores da velocidade do rb

        if (pos.z >= maxBoundZ || pos.z <= minBoundZ) // se pos.z for menor/maior/igual os valores limites
            vel.z = 0f; // a velocidade é zerada, força para de ser adicionada
        
        pos.z = Mathf.Clamp(pos.z, minBoundZ, maxBoundZ); // determina os valores limites de z
        
        // faz com que o player apareça do lado oposto da tela caso ele saia da mesma pela horizontal
        if (pos.x >= boundX)        pos.x = -boundX;
        else if (pos.x <= -boundX)  pos.x = boundX;

        _playerRb.linearVelocity = vel; // valor da velocidade de rb se torna o valor da variável vel
        transform.position = pos; // valor do transform.position se torna o valor da variável pos
    }
}

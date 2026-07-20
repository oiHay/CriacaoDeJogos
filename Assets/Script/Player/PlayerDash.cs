using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip dashCooldownSound;

    private PlayerHealth _health;
    private Rigidbody _rb;
    private bool _canDash = true;
    private bool _isGameActive;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
        _rb = GetComponent<Rigidbody>();
    }

    public void SetGameState(GameState state) => _isGameActive = state == GameState.Playing;

    private void Update()
    {
        if (!_isGameActive || !_canDash)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AudioManager.Instance.PlaySfx(dashCooldownSound);
            
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _canDash = false;
        _health.SetInvulnerable(true);
        
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (dir == Vector3.zero) dir = Vector3.forward;

        _rb.AddForce(dir * dashForce, ForceMode.Impulse);
        
        AudioManager.Instance.PlaySfx(dashSound);

        yield return new WaitForSeconds(dashDuration);
        _health.SetInvulnerable(false);

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
}

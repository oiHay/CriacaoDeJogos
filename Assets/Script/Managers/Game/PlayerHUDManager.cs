using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthBar;

    private void OnEnable() => playerHealth.OnHealthChanged += UpdateHealth;
    private void OnDisable() => playerHealth.OnHealthChanged -= UpdateHealth;

    private void Start()
    {
        healthBar.value = playerHealth.maxHealth;
    }

    private void UpdateHealth(int current, int max)
    {
        healthBar.value = (float)current / max;
    }
}

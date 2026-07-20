using UnityEngine;
using UnityEngine.UI;

public class BossHUDManager : MonoBehaviour
{
    [SerializeField] private BossBehaviour boss;
    [SerializeField] private Slider healthBar;

    [Header("Cor por fase")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Color phase1Color = new Color(0.95f, 0.75f, 0.2f);
    [SerializeField] private Color phase2Color = new Color(0.95f, 0.45f, 0.2f);
    [SerializeField] private Color phase3Color = new Color(0.9f, 0.25f, 0.25f);

    private void OnEnable()
    {
        boss.OnHealthChanged += UpdateHealth;
        boss.OnPhaseChanged += UpdatePhaseColor;
        boss.OnBossDestroyed += HandleBossDestroyed;
    }

    private void OnDisable()
    {
        if (boss == null) return;

        boss.OnHealthChanged -= UpdateHealth;
        boss.OnPhaseChanged -= UpdatePhaseColor;
        boss.OnBossDestroyed -= HandleBossDestroyed;
    }

    private void Start()
    {
        healthBar.value = 1f;
        UpdatePhaseColor(BossPhase.Phase1);
    }

    private void UpdateHealth(float current, float max)
    {
        healthBar.value = current / max;
    }

    private void UpdatePhaseColor(BossPhase phase)
    {
        if (fillImage == null) return;

        fillImage.color = phase switch
        {
            BossPhase.Phase1 => phase1Color,
            BossPhase.Phase2 => phase2Color,
            BossPhase.Phase3 => phase3Color,
            _                => phase1Color
        };
    }

    private void HandleBossDestroyed()
    {
        gameObject.SetActive(false);
    }
}

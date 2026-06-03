using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpCardUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    private PowerUpSO _data;

    public void Setup(PowerUpSO powerUp)
    {
        _data = powerUp;

        if (icon != null)            icon.sprite          = powerUp.icon;
        if (nameText != null)        nameText.text        = powerUp.powerUpName;
        if (descriptionText != null) descriptionText.text = powerUp.description;

        // log any missing references so you know which card is broken
        if (icon == null)            Debug.LogError("PowerUpCardUI: icon not assigned!",            this);
        if (nameText == null)        Debug.LogError("PowerUpCardUI: nameText not assigned!",        this);
        if (descriptionText == null) Debug.LogError("PowerUpCardUI: descriptionText not assigned!", this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PowerUpManager.Instance.OnPowerUpChosen(_data);
    }
}

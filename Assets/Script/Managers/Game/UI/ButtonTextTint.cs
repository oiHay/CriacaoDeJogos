using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTextTint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TMP_Text label;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData) => Tint(_button.colors.highlightedColor);

    public void OnPointerExit(PointerEventData eventData) => Tint(_button.colors.normalColor);

    public void OnPointerDown(PointerEventData eventData) => Tint(_button.colors.pressedColor);

    public void OnPointerUp(PointerEventData eventData) => Tint(_button.colors.highlightedColor);

    private void OnDisable()
    {
        if (label != null && _button != null)
            label.CrossFadeColor(Color.white, 0f, true, true);
    }

    private void Tint(Color color)
    {
        if (label == null || !_button.interactable) return;

        label.CrossFadeColor(color * _button.colors.colorMultiplier, _button.colors.fadeDuration, true, true);
    }
}
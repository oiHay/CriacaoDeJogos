using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip btnHovered;
    
    private IPointerEnterHandler _pointerEnterHandlerImplementation;
    
    public void OnPointerEnter(PointerEventData eventData)
    { 
        AudioManager.Instance.PlaySfx(btnHovered);
    }
}

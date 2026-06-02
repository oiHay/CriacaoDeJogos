using UnityEngine;

public class PowerUpPanelController : MonoBehaviour
{
    [SerializeField] private PowerUpCardUI[] cards;

    public void Show(PowerUpSO[] options)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Setup(options[i]);
        }
    }
}

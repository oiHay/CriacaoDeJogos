using UnityEngine;

public class PowerUpPanelController : MonoBehaviour
{
    [SerializeField] private PowerUpCardUI[] cards;

    public void Show(PowerUpSO[] options)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            bool hasOption = i < options.Length;
            cards[i].gameObject.SetActive(hasOption);

            if (hasOption)
                cards[i].Setup(options[i]);
        }
    }
}

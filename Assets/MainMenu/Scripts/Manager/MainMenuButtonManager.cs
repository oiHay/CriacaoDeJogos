using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] private MenuUIManager.MainMenuButtons buttonType;

    public void ButtonClicked()
    {
        MenuUIManager.Instance.ButtonClicked(buttonType);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class UIButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onClicked;

    public void Click() => onClicked.Invoke();
}

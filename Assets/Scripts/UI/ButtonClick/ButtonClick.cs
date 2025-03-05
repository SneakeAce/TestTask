using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private Button _button;

    private PlayerInput _playerInput;

    private TakingItem _takingItem;
    private DiscardItem _discardItem;

    [Inject]
    private void Construct(TakingItem takingItem, DiscardItem discardItem, PlayerInput playerInput)
    {
        _playerInput = playerInput;

        _button.gameObject.SetActive(false); 

        _takingItem = takingItem;
        _discardItem = discardItem;

        Initialization();
    }

    public void ClickButton()
    {
        _discardItem.DropItem();

        _takingItem.RemoveItem();

        DisableButton();
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy in ButtonClick");
        _button.gameObject.SetActive(false);
        _takingItem.EnableButton -= OnEnableButton;
    }

    private void Initialization()
    {
        _takingItem.EnableButton += OnEnableButton;
    }

    private void OnEnableButton()
    {
        Debug.Log("OnEnableButton");
        _button.gameObject.SetActive(true);
        _playerInput.TakingItem.PickItem.Disable();
    }

    private void DisableButton()
    {
        _button.gameObject.SetActive(false);
        _playerInput.TakingItem.PickItem.Enable();
    }
}

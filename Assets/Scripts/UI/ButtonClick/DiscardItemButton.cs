using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DiscardItemButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private PlayerInput _playerInput;

    private LazyInject<TakingItem> _lazyTakingItem;
    private TakingItem _takingItem;

    [Inject]
    private void Construct(LazyInject<TakingItem> takingItem, PlayerInput playerInput)
    {
        _playerInput = playerInput;

        _lazyTakingItem = takingItem;
    }

    public event Action DropItem;
    public event Action<GameObject> SetDroppingItem;

    private void Start()
    {
        _takingItem = _lazyTakingItem.Value;

        Initialization();

        _button.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _button.gameObject.SetActive(false);
        _takingItem.EnableButton -= OnEnableButton;
    }

    public void OnClickButton()
    {
        SetDroppingItem?.Invoke(_takingItem.CurrentItem);

        DropItem?.Invoke();

        DisableButton();
    }

    private void Initialization()
    {
        _takingItem.EnableButton += OnEnableButton;
    }

    private void OnEnableButton()
    {
        _button.gameObject.SetActive(true);
        _playerInput.TakingItem.PickItem.Disable();
    }

    private void DisableButton()
    {
        _button.gameObject.SetActive(false);
        _playerInput.TakingItem.PickItem.Enable();
    }
}

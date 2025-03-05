using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakingItem : IDisposable, IInitializable
{
    private const float MaxRangeRaycast = 2f;

    // Сделать здесь поле с кнопкой интерфейса
    private Camera _camera;
    private LayerMask _pickableItemLayer = 1 << 6;

    private PlayerController _player;
    private PlayerInput _playerInput;

    private DiscardItem _discardItem;

    private GameObject _currentItem;

    public GameObject CurrentItem { get => _currentItem; set => _currentItem = value; }

    public event Action EnableButton;

    public TakingItem(Camera camera, PlayerController player, PlayerInput playerInput, DiscardItem discardItem)
    {
        _camera = camera;
        _player = player;
        _playerInput = playerInput;

        _discardItem = discardItem;

        Initialize();
    }

    public void Initialize()
    {
        _playerInput.TakingItem.PickItem.performed += OnTryPickItem;
    }

    public void Dispose()
    {
        _playerInput.TakingItem.PickItem.performed -= OnTryPickItem;
    }

    public void RemoveItem() => _currentItem = null;

    private void OnTryPickItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryPickItem();
        }
    }

    private void TryPickItem()
    {
        Debug.Log("TryPickItem");
        Vector2 touchPosition = _playerInput.TakingItem.PickItem.ReadValue<Vector2>();

        Ray ray = _camera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxRangeRaycast, _pickableItemLayer))
        {
            Debug.Log("Raycast target == " + hitInfo.collider.name + " / target layer == " + hitInfo.collider.gameObject.layer);
            if ((1 << hitInfo.collider.gameObject.layer) == _pickableItemLayer)
            {
                Debug.Log("Call PickUpItem");
                PickUpItem(hitInfo.collider.gameObject);

                EnableButton?.Invoke();
            }
            else
            {
                return;

            }

        }
    }

    private void PickUpItem(GameObject item)
    {
        Debug.Log("PickUpItem");
        if (_currentItem != null)
            return;

        _currentItem = item;
        _discardItem.GetItem(_currentItem);

        _currentItem.transform.SetParent(_player.HandPosition);
        _currentItem.transform.localPosition = Vector3.zero;
        _currentItem.transform.localRotation = Quaternion.identity;
        _currentItem.GetComponent<Rigidbody>().isKinematic = true;
        _currentItem.GetComponent<MeshCollider>().enabled = false;
    }

}

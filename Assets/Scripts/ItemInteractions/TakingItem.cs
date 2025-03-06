using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakingItem : IDisposable, IInitializable
{
    private Camera _camera;
    private LayerMask _pickableItemLayer = 1 << 6;

    private PlayerController _player;
    private PlayerInput _playerInput;

    private GameObject _currentItem;

    private const float MaxRangeRaycast = 2f;
    public GameObject CurrentItem { get => _currentItem; set => _currentItem = value; }

    public TakingItem(Camera camera, PlayerController player, PlayerInput playerInput, DiscardItemButton discardItemButton)
    {
        _camera = camera;
        _player = player;
        _playerInput = playerInput;

        discardItemButton.DropItem += RemoveItem;

        Initialize();
    }

    public event Action EnableButton;

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
        if (_currentItem != null)
            return;

        if (context.performed)
        {
            TryPickItem();
        }
    }

    private void TryPickItem()
    {
        Vector2 touchPosition = _playerInput.TakingItem.PickItem.ReadValue<Vector2>();

        Ray ray = _camera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxRangeRaycast, _pickableItemLayer))
        {
            if ((1 << hitInfo.collider.gameObject.layer) == _pickableItemLayer)
            {
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
        if (_currentItem != null)
            return;

        _currentItem = item;

        _currentItem.transform.SetParent(_player.HandPosition);
        _currentItem.transform.localPosition = Vector3.zero;
        _currentItem.transform.localRotation = Quaternion.identity;

        Rigidbody itemRb = _currentItem.GetComponent<Rigidbody>();
        if (itemRb != null)
            itemRb.isKinematic = true;
        
        MeshCollider itemMeshCollider = _currentItem.GetComponent<MeshCollider>();
        if (itemMeshCollider != null)
            itemMeshCollider.enabled = false;
    }

}

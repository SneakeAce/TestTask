using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakingItem : IDisposable, IInitializable
{
    private const float MaxRangeRaycast = 10f;

    // Сделать здесь поле с кнопкой интерфейса
    private Camera _camera;
    private LayerMask _pickableItemLayer = 1 << 6;

    private PlayerController _player;
    private PlayerInput _playerInput;

    private GameObject _currentItem;

    public TakingItem(Camera camera, PlayerController player, PlayerInput playerInput)
    {
        Debug.Log("TakingItem Construct");
        _camera = camera;
        _player = player;
        _playerInput = playerInput;
        // Здесь сделать инициализацию кнопки
        Initialize();
    }

    public void Initialize()
    {
        _playerInput.TakingItem.PickItem.performed += OnTryPickItem;

        Debug.Log("TakingItem / Initialize");
    }

    public void Dispose()
    {
        _playerInput.TakingItem.PickItem.performed -= OnTryPickItem;

        Debug.Log("TakingItem / Dispose");
    }

    private void OnTryPickItem(InputAction.CallbackContext context)
    {
        Debug.Log("OnTryPickItem");
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
            if (hitInfo.collider.gameObject.layer != _pickableItemLayer)
            {
                Debug.Log("Hit Collider!!!");
                return;
            }
            else
            {
                PickUpItem(hitInfo.collider.gameObject);

                // Вызвать событие для того, чтобы появилась кнопка сброса предмета
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
        _currentItem.GetComponent<Rigidbody>().isKinematic = true;
    }

}

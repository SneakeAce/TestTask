using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _touchAreas;

    private PlayerController _player;
    private GameObject _playerHead;

    private PlayerInput _playerInput;

    private CinemachineVirtualCamera _virtualCamera;

    private CinemachinePOV _povCamera;

    private Vector2 _lookInput;

    private float _sensitivityCamera;

    private string _playerHeadName = "Head";

    private bool _isPickItemEnabled;

    private const float MinValueForOffsetCamera = 0.01f;
    private const float MinValueCoordinate = 0f;

    [Inject]
    public void Constuct(PlayerController player, PlayerInput playerInput, 
        CinemachineVirtualCamera virtualCamera, CameraConfig cameraConfig)
    {
        _player = player;
        _playerInput = playerInput;
        _virtualCamera = virtualCamera;

        _sensitivityCamera = cameraConfig.SensitivityCamera;

        Initialize();
    }

    public void Initialize()
    {
        _povCamera = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        _virtualCamera.Follow = _player.GetComponentInChildren<Transform>().Find(_playerHeadName);
        _virtualCamera.LookAt = null;
    }

    private void Update()
    {
        _lookInput = _playerInput.PlayerLooking.LookAround.ReadValue<Vector2>();

        if (IsTouchInArea() == false)
            return;

        if (_lookInput.sqrMagnitude < MinValueForOffsetCamera)
        {
            if (_isPickItemEnabled == false)
            {
                _playerInput.TakingItem.PickItem.Enable();
                _isPickItemEnabled = true;
            }

            return;
        }

        if (_isPickItemEnabled)
        {
            _playerInput.TakingItem.PickItem.Disable();
            _isPickItemEnabled = false;
        }

        _playerInput.TakingItem.PickItem.Disable();

        _povCamera.m_HorizontalAxis.Value += _lookInput.x * _sensitivityCamera;
        _povCamera.m_VerticalAxis.Value -= _lookInput.y * _sensitivityCamera;

        _player.transform.rotation = Quaternion.Euler(_povCamera.m_VerticalAxis.Value, _povCamera.m_HorizontalAxis.Value, MinValueCoordinate);
    }

    private bool IsTouchInArea()
    {
        if (_touchAreas == null || _touchAreas.Count == 0)
            return true;

        Vector2 screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        foreach (RectTransform area in _touchAreas)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(area, screenPosition))
                return true;
        }

        return false;
    }
}

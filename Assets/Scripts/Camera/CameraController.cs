using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    private const float MinValueForOffsetCamera = 0.01f;
 
    private PlayerController _player;
    private PlayerInput _playerInput;

    private CinemachineVirtualCamera _virtualCamera;

    private CinemachinePOV _povCamera;
    private Vector2 _lookInput;

    private float _sensitivityCamera;

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

        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
    }

    private void Update()
    {
        _lookInput = _playerInput.PlayerLooking.LookAround.ReadValue<Vector2>();

        if (_lookInput.sqrMagnitude < MinValueForOffsetCamera)
            return;

        _povCamera.m_HorizontalAxis.Value += _lookInput.x * _sensitivityCamera;

        _povCamera.m_VerticalAxis.Value -= _lookInput.y * _sensitivityCamera;
    }
}

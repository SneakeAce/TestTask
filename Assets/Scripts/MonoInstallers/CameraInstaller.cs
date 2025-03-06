using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CameraConfig _config;

    public override void InstallBindings()
    {
        BindCameraConfig();

        BindCamera();
    }

    private void BindCamera()
    {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CinemachineVirtualCamera>().FromInstance(_virtualCamera).AsSingle().NonLazy();
    }

    private void BindCameraConfig()
    {
        Container.Bind<CameraConfig>().FromInstance(_config).AsSingle().NonLazy();
    }
}

using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private DiscardItemButton _discardItemButton;
    [SerializeField] private ButtonOpenDoor _openDoorButton;
    public override void InstallBindings()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        Container.Bind<DiscardItemButton>().FromInstance(_discardItemButton).AsSingle().NonLazy();
        Container.Bind<ButtonOpenDoor>().FromInstance(_openDoorButton).AsSingle().NonLazy();
    }
}

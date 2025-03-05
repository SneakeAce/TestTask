using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private ButtonClick _buttonClick;

    public override void InstallBindings()
    {
        BindButtonClick();
    }

    private void BindButtonClick()
    {
        Container.Bind<ButtonClick>().FromInstance(_buttonClick).AsSingle().NonLazy();
    }
}

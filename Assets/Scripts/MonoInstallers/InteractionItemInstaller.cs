using Zenject;

public class InteractionItemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindTakingItem();

        BindDiscardItem();
    }

    private void BindTakingItem()
    {
        Container.BindInterfacesAndSelfTo<TakingItem>().AsSingle().NonLazy();
    }

    private void BindDiscardItem()
    {
        Container.Bind<DiscardItem>().AsSingle().NonLazy();
    }
}

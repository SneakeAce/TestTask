using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerStatsConfig _playerStatsConfig;

    public override void InstallBindings()
    {
        BindConfig();

        BindComponents();

        CreateAndBindPlayer();

        Debug.Log("All Binds Done");
    }

    private void BindConfig()
    {
        Container.Bind<PlayerStatsConfig>().FromInstance(_playerStatsConfig).AsSingle();
    }

    private void BindComponents()
    {
        Container.Bind<MovementComponent>().AsSingle().NonLazy();
    }

    private void CreateAndBindPlayer()
    {
        PlayerController player = Container.InstantiatePrefabForComponent<PlayerController>(_playerStatsConfig.PlayerPrefab,
            _playerStatsConfig.SpawnCoordinate, Quaternion.identity, null);

        Container.BindInterfacesAndSelfTo<PlayerController>().FromInstance(player).AsSingle().NonLazy();
    }
}

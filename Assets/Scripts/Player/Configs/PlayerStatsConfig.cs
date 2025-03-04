using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PlayerConfig", fileName = "PlayerStatsConfig")]
public class PlayerStatsConfig : ScriptableObject
{
    [field: SerializeField] public PlayerController PlayerPrefab;
    [field: SerializeField, Range(1f, 10f)] public float MovementSpeed { get; private set; }
    [field: SerializeField] public Vector3 SpawnCoordinate { get; private set; }
}

using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour, IPlayer
{
    private MovementComponent _movementComponent;
    private PlayerStatsConfig _playerStatsConfig;

    [SerializeField] private Transform _handPosition;

    private Rigidbody _rigidbody;
    private Collider _collider;

    public Rigidbody Rigidbody => _rigidbody;
    public Collider Collider => _collider;
    public Transform HandPosition => _handPosition;

    [Inject]
    private void Construct(MovementComponent movementComponent, PlayerStatsConfig playerStatsConfig)
    {
        _movementComponent = movementComponent;
        _playerStatsConfig = playerStatsConfig;

        Initialize();
    }

    private void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _movementComponent.Initialization(this, _playerStatsConfig.MovementSpeed);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _movementComponent.FixedUpdate();
    }

}

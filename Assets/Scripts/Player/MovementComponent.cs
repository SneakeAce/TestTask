using UnityEngine;

public class MovementComponent
{
    private PlayerController _player;
    private PlayerInput _playerInput;

    private float _speed;
    public MovementComponent(PlayerInput playerInput)
    {
        _playerInput = playerInput;

        _playerInput.Enable();
    }

    public void Initialization(PlayerController player, float speed)
    {
        _player = player;
        _speed = speed;
    }

    public void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movementInput = _playerInput.PlayerMoving.Move.ReadValue<Vector2>();

        Vector3 directionMove = new Vector3(movementInput.x, 0f, movementInput.y);

        _player.Rigidbody.velocity = new Vector3(directionMove.x * _speed, 0f, directionMove.z * _speed);
    }

}

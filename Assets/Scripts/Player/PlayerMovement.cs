using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [Header("References")]
    [SerializeField] private Tilemap wallsTilemap;

    private Rigidbody2D rb2D;
    private bool _canMove;

    private Vector2 _inputDirection;
    private Vector2 _direction;

    private Vector3Int _currentTile;
    private Vector3 _currentTileCenter;
    private Vector3Int _nextInputTile;
    private Vector3Int _nextTile;
    private Vector3 _nextTileCenter;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 move = InputManager.instance.input.Player.Move.ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            if (!(move.x != 0 && move.y != 0))
            {
                _inputDirection = move;
                Debug.Log(_inputDirection);
            }
        }

        _currentTile = wallsTilemap.WorldToCell(rb2D.position);
        _currentTileCenter = wallsTilemap.GetCellCenterWorld(_currentTile);

        _nextInputTile = wallsTilemap.WorldToCell(_currentTileCenter + (Vector3)_inputDirection);
        if (Vector2.Distance(rb2D.position, _currentTileCenter) <= 0.001f)
        {
            if (!wallsTilemap.HasTile(_nextInputTile))
            {
                _direction = _inputDirection;
            }

            _nextTile =  wallsTilemap.WorldToCell(_currentTileCenter + (Vector3)_direction);
            _canMove = !wallsTilemap.HasTile(_nextTile);
        }
        _nextTileCenter = wallsTilemap.GetCellCenterWorld(_nextTile);
    }
    private void FixedUpdate()
    {
        if (_canMove)
        {
            rb2D.position = Vector2.MoveTowards(rb2D.position, _nextTileCenter, speed * Time.fixedDeltaTime);
        }
        else
        {
            rb2D.position = Vector2.MoveTowards(rb2D.position, _currentTileCenter, speed * Time.fixedDeltaTime);
        }
    }
}

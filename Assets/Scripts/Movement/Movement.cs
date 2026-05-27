using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Settings")]
    public float Speed;
    [Header("References")]
    public Tilemap wallsTilemap;
    [SerializeField] private Rigidbody2D Rb2D;

    [HideInInspector] public bool IsPaused;

    private bool _canMove;

    private Vector2Int _inputDirection;

    private Vector2Int _direction;
    [HideInInspector] public Vector2Int Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;

            if (!IsPaused)
                MovementDirectionChanged?.Invoke(_direction);
        }
    }

    [HideInInspector] public Vector3Int CurrentTile;
    private Vector3 _currentTileCenter;
    private Vector3Int _nextInputTile;
    private Vector3Int _nextTile;
    private Vector3 _nextTileCenter;

    [HideInInspector] public bool CloseToCenter;
    public event Action<Vector2Int> MovementDirectionChanged;
    public event Action MovementDirectionFlipped;

    private void Update()
    {
        CurrentTile = wallsTilemap.WorldToCell(transform.position);
        _currentTileCenter = wallsTilemap.GetCellCenterWorld(CurrentTile);

        _nextInputTile = CurrentTile + (Vector3Int)_inputDirection;
        CloseToCenter = Vector2.Distance(transform.position, _currentTileCenter) <= 0.001f;
        if (CloseToCenter)
        {
            if (!wallsTilemap.HasTile(_nextInputTile))
            {
                Direction = _inputDirection;
            }

            _nextTile = CurrentTile + (Vector3Int)Direction;
            _canMove = !wallsTilemap.HasTile(_nextTile);
        }
        _nextTileCenter = wallsTilemap.GetCellCenterWorld(_nextTile);
    }
    private void FixedUpdate()
    {
        if (_canMove)
        {
            if (!IsPaused)
            Rb2D.position = Vector2.MoveTowards(Rb2D.position, _nextTileCenter, Speed * Time.fixedDeltaTime);
        }
        else
        {
            Rb2D.position = Vector2.MoveTowards(Rb2D.position, _currentTileCenter, Speed * Time.fixedDeltaTime);
        }
    }

    public void SetDirection(Vector2Int dir)
    {   
        _inputDirection = dir;
    }
    public void FlipDirection()
    {
        _inputDirection = -_direction;
        Direction = -_direction; 
        MovementDirectionFlipped?.Invoke();
    }
}

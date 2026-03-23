using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [Header("References")]
    public Tilemap wallsTilemap;
    [SerializeField] private Rigidbody2D Rb2D;

    [HideInInspector] public bool IsPaused;

    private bool _canMove;

    private Vector2 _inputDirection;
    private Vector2 _direction;

    [HideInInspector] public Vector3Int CurrentTile;
    private Vector3 _currentTileCenter;
    private Vector3Int _nextInputTile;
    private Vector3Int _nextTile;
    private Vector3 _nextTileCenter;

    [HideInInspector] public bool CloseToCenter;

    private void Update()
    {
        CurrentTile = wallsTilemap.WorldToCell(Rb2D.position);
        _currentTileCenter = wallsTilemap.GetCellCenterWorld(CurrentTile);

        _nextInputTile = wallsTilemap.WorldToCell(_currentTileCenter + (Vector3)_inputDirection);
        CloseToCenter = Vector2.Distance(Rb2D.position, _currentTileCenter) <= 0.001f;
        if (CloseToCenter)
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
            if (!IsPaused)
            Rb2D.position = Vector2.MoveTowards(Rb2D.position, _nextTileCenter, speed * Time.fixedDeltaTime);
        }
        else
        {
            Rb2D.position = Vector2.MoveTowards(Rb2D.position, _currentTileCenter, speed * Time.fixedDeltaTime);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        _inputDirection = dir;
    }
}

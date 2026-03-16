using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [Header("References")]
    [SerializeField] private Tilemap walls;
    [SerializeField] private Rigidbody2D rb;

    private float _currentSpeed;

    private Vector2 _inputDir;
    private Vector2 _lastInputDir;
    private Vector2 _dir = Vector2.right;

    private Vector3 center;

    private Vector3Int _currentCell;
    private Vector3Int _nextCellDir;
    private Vector3Int _nextCellInput;

    private bool _canMove;

    private void Update()
    {
        _inputDir = InputManager.instance.input.Player.Move.ReadValue<Vector2>();
        if (_inputDir != Vector2.zero)
        {
            if (!(_inputDir.x != 0 && _inputDir.y != 0))
            {
                _lastInputDir = Vector3.Normalize(_inputDir);                
            }
        }

        _currentCell = walls.WorldToCell(rb.position);

        _nextCellInput = _currentCell + Vector3Int.RoundToInt(_lastInputDir);
        if (!walls.HasTile(_nextCellInput))
        {
            _dir = _lastInputDir;
        }

        center = walls.GetCellCenterWorld(_currentCell);
        if (Vector3.Distance(rb.position, center) <= 0.001f)
        {
            _nextCellDir = _currentCell + Vector3Int.RoundToInt(_dir);
            _canMove = !walls.HasTile(_nextCellDir);
        }

        if (_dir.y != 0)
        {
            _currentSpeed = verticalSpeed;
        }
        else
        {
            _currentSpeed = horizontalSpeed;
        }
    } 
    private void FixedUpdate()
    {
        if (_canMove)
        {
            Vector3 nextCenter = walls.GetCellCenterWorld(_nextCellDir);
            rb.position = Vector3.MoveTowards(rb.position, nextCenter, _currentSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 nextCenter = walls.GetCellCenterWorld(_nextCellDir);
            rb.position = Vector3.MoveTowards(rb.position, center, _currentSpeed * Time.deltaTime);
        }
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [Header("References")]
    [SerializeField] private Tilemap walls;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 _inputDir;
    private Vector2 _lastInputDir;
    private Vector2 _dir;

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
            _lastInputDir = Vector3.Normalize(_inputDir);
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
    }
    private void FixedUpdate()
    {
        if (_canMove)
        {
            Vector3 nextCenter = walls.GetCellCenterWorld(_nextCellDir);
            rb.position = Vector3.MoveTowards(rb.position, nextCenter, speed * Time.deltaTime);
        }
        else
        {
            rb.position = Vector3.MoveTowards(rb.position, center, speed * Time.deltaTime);
        }
    }
}

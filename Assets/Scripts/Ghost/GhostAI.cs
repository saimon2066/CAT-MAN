using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("References")]
    public Movement Movement;

    private Vector3Int _destination;
    private Vector2Int[] _directions = {new(0, 1), new(-1, 0), new(0, -1), new(1, 0)}; // Up, Left, Down, Right

    private Vector3Int _lastTile;

    private bool _randomize;

    private void OnDisable()
    {
        Movement.MovementDirectionFlipped -= OnMovementDirectionFlipped;
        Movement.MovementCloseToCenter -= OnMovementCloseToCenter;
    }
    private void OnEnable()
    {
        Movement.MovementDirectionFlipped += OnMovementDirectionFlipped;
        Movement.MovementCloseToCenter += OnMovementCloseToCenter;
    }

    private void RunAlgorithm()
    {
        Dictionary<Vector2Int, float> possibleDirectionsAndDistance = GetPossibleDirectionsAndDistance();

        if (_randomize)
        {
            int rand = Random.Range(0, possibleDirectionsAndDistance.Count);
            Vector2Int dir = possibleDirectionsAndDistance.Keys.ElementAt(rand);
            Movement.SetDirection(dir);
        }
        else
        {
            float min = possibleDirectionsAndDistance.Values.Min();

            foreach (Vector2Int dir in _directions)
            {
                if (possibleDirectionsAndDistance.TryGetValue(dir, out float distance) && distance == min)
                {
                    Movement.SetDirection(dir);
                    break;  
                }   
            }   
        }
    }
    private Dictionary<Vector2Int, float> GetPossibleDirectionsAndDistance()
    {
        Dictionary<Vector2Int, float> possible = new();
        
        foreach (Vector2Int dir in _directions)
        {
            Vector3Int next = Movement.CurrentTile + (Vector3Int)dir;

            if (!Movement.wallsTilemap.HasTile(next) && dir != -Movement.Direction)
            {                
                float distance = (next - _destination).sqrMagnitude;
                possible.Add(dir, distance);
            }
        }

        if (possible.Count == 0)
        {
            Vector2Int reverse = -Movement.Direction;
            Vector3Int next = Movement.CurrentTile + (Vector3Int)reverse;

            if (!Movement.wallsTilemap.HasTile(next))
            {
                float distance = (next - _destination).sqrMagnitude;
                possible.Add(reverse, distance);   
            }
        }

        return possible;
    }

    public void SetDestination(Vector3Int destination)
    {
        _destination = destination;
    }
    public void SetRandomization(bool randomize)
    {
        _randomize = randomize;
    }
    private void OnMovementDirectionFlipped()
    {
        _lastTile = Movement.CurrentTile;
    }
    private void OnMovementCloseToCenter()
    {
        if (Movement.CurrentTile != _lastTile || !Movement.CanMove)
        {
            _lastTile = Movement.CurrentTile;
            RunAlgorithm();
        }
    }
}
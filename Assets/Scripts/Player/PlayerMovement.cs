using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Movement movement;

    private void Update()
    {
        Vector2 move = InputManager.instance.input.Player.Move.ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            if (!(move.x != 0 && move.y != 0))
            {
                movement.SetDirection(move);
            }
        }
    }
}

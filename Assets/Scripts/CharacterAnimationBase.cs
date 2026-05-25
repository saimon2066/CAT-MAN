using UnityEngine;

public abstract class CharacterAnimationBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Movement movement;
    [SerializeField] protected Animator animator;

    public virtual void OnEnable()
    {
        movement.MovementDirectionChanged += OnMovementDirectionChanged;
    }
    public virtual void OnDisable()
    {
        movement.MovementDirectionChanged -= OnMovementDirectionChanged;
    }

    public virtual void OnMovementDirectionChanged(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (dir == Vector2Int.right)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (dir == Vector2Int.left)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (dir == Vector2Int.down)
        {
            animator.SetInteger("Direction", 3);
        }
    }
}

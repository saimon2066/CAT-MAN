using UnityEngine;

public class ButtonFrameAnimation : MonoBehaviour
{
    [SerializeField] private string parameter;
    [SerializeField] private Animator animator;

    public void SetParameter(bool value)
    {
        animator.SetBool(parameter, value);
    }
}

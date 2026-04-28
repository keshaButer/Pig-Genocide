using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public float Death()
    {
        animator.SetTrigger("isDeath");
        return 2;
    }
}

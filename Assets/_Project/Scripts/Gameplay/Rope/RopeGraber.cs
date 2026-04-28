using UnityEngine;

public class RopeGraber : MonoBehaviour, IInteractableObject
{
    private RopeControl ropeControl;

    private void Awake() => ropeControl = transform.parent.GetComponent<RopeControl>();

    public void Interact()
    {
        ropeControl.UseRope(transform);
    }
}

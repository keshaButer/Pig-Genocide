public class RopeGraber : InteractableObject
{
    private RopeControl ropeControl;
    private void Awake() => ropeControl = transform.parent.GetComponent<RopeControl>();
    public override void Interact()
    {
        print("USE ROPE");
        ropeControl.UseRope(transform);
    }
}

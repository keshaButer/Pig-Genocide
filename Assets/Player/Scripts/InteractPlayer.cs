using UnityEngine;

public class InteractPlayer : MonoBehaviour
{
    [SerializeField] private float _circleRadius;
    [SerializeField] private Transform _circleCenter;
    [SerializeField] private InputPlayerMovementConfig _inputConfig;

    private RaycastHit2D hit;

    private void Update()
    {
        if (Input.GetKeyDown(_inputConfig.interactKey))
        {
            Debug.Log("Pressed Interect");
            Collider2D[] colliders =  Physics2D.OverlapCircleAll(_circleCenter.position, _circleRadius);
            if (colliders != null)
            {
                foreach (Collider2D collider in colliders)
                {
                    IInteractableObject interactableObject;
                    if (collider.TryGetComponent<IInteractableObject>(out interactableObject))
                    {
                        Debug.Log("ITS interactableObject");
                        interactableObject.Interact();
                        break;
                    }
                }
            }
        }
    }
}

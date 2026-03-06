using UnityEngine;

public class InteractPlayer : MonoBehaviour
{
    [SerializeField] private float circleRasius;
    private InputPlayerMovementConfig inputConfig;
    private Transform rayPoint;
    private RaycastHit2D hit;
    private void Start()
    {
        inputConfig = GetComponent<MovementPlayer>().inputConfig;
        rayPoint = transform.GetChild(1).GetChild(3);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.GetComponent<InteractableObject>() && other.tag == "Item")
        {
            InteractableObject interactableObject = other.transform.GetComponent<InteractableObject>();
            // interactableObject.interacter = gameObject;
            interactableObject.Interact();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(inputConfig.interactKey))
        {
            Collider2D[] colliders =  Physics2D.OverlapCircleAll(rayPoint.position, circleRasius);
            if (colliders.Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (collider.transform.GetComponent<InteractableObject>())
                    {
                        InteractableObject interactableObject = collider.transform.GetComponent<InteractableObject>();
                        // interactableObject.interacter = gameObject;
                        interactableObject.Interact();
                    }
                }
            }
        }
    }
}

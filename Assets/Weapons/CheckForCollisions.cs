using UnityEngine;

public abstract class CheckForCollisions : MonoBehaviour
{
    protected abstract void OnTriggerEnter2D(Collider2D collision);
}

using UnityEngine;

public class ControlDisabler : MonoBehaviour
{
    public bool isUsing = false;

    public void DisableControl()
    {
        GetComponent<MovementPlayer>().isInput = false;
        GetComponent<Rigidbody2D>().simulated = false;
    }
    public void EnableControl()
    {
        GetComponent<MovementPlayer>().isInput = true;
        GetComponent<Rigidbody2D>().simulated = true;
    }
}

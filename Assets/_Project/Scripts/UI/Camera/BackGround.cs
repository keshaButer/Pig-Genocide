using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] float accel;
    private Transform camera;
    private void Start()
    {
        camera = Camera.main.transform;
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(camera.position.x, 0, 0), accel);
    }
}

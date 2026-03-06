using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathFloor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Restart();
        }
        else Destroy(other.gameObject);
    }
    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}

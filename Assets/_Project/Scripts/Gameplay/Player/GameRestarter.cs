using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DeathHandler))]
public class GameRestarter : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    private DeathHandler _deathHandler;

    private void Awake()
    {
        _deathHandler = GetComponent<DeathHandler>();
        _deathHandler.OnDeath += RestartGame;
    }

    private void RestartGame() => SceneManager.LoadScene(_sceneIndex);

    private void OnDisable()
    {
        if (_deathHandler != null)
            _deathHandler.OnDeath -= RestartGame;
    }
}
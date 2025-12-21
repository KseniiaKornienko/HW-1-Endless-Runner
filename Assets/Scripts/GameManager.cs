using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController movement;
    public float levelRestartDelay = 2f;
    [SerializeField] private GameObject losePanel;

    void Start() {
        losePanel.SetActive(false);
    }

    public void EndGame() {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController movement;
    public float levelRestartDelay = 2f;
    [SerializeField] private GameObject losePanel;
    public static bool gameOver;

    void Start() {
        losePanel.SetActive(false);
        gameOver = false;
    }

    public void EndGame() {
        // movement.enabled = false;
        // Invoke("RestartLevel", levelRestartDelay);
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    // void RestartLevel() {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }

    // public static void Damage (int damageCount) {
    //     playerHealth -= damageCount;
    //     Debug.Log(playerHealth);
    //     if (playerHealth <= 0) {
    //         gameOver = true;
    //     }
    // }

    // void Update() {
    //     if (gameOver) {
    //         EndGame();
    //     }
    // }
}

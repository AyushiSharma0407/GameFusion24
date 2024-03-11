using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText; // Added to display final score
    public GameObject restartButton;
    public TextMeshProUGUI scoreText;

    private int score;

    private void Start()
    {
        // Initialize score
        score = 0;

        // Hide game over text, final score, and restart button initially
        gameOverText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false); // Initially hide final score
        restartButton.SetActive(false);
        PlayerController.OnPlayerDeath += PlayerDied;
    }

    public void PlayerDied()
    {
        // Show Game Over text, final score, and restart button
        gameOverText.gameObject.SetActive(true);
        finalScoreText.gameObject.SetActive(true); // Show final score
        restartButton.SetActive(true);

        gameOverText.text = "Game Over";
        finalScoreText.text = "Final Score: " + score.ToString(); // Display final score

        // Stop the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Resume game
        Time.timeScale = 1f;

        // Reload the current scene asynchronously to restart the game
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex).completed += LoadSceneComplete;

        // Reset score
        score = 0;
        UpdateScore();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the player death event to prevent memory leaks
        PlayerController.OnPlayerDeath -= PlayerDied;
    }

    public void IncreaseScore(int amount)
    {
        // Increase the score
        score += amount;
        UpdateScore();
    }

    private void UpdateScore()
    {
        // Update the score text
        scoreText.text = "Score: " + score.ToString();
    }

    private void LoadSceneComplete(AsyncOperation asyncOperation)
    {
        // Scene loading completed
        Debug.Log("Scene reloaded");
    }
}

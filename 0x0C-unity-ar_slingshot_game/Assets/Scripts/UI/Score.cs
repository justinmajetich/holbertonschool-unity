using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highscoreText;

    int score = 0;
    int[] highscores = new int[3];

    private void OnEnable() {
        GameManager.onStartGame += ResetScore;
        GameManager.onGameOver += UpdateHighscores;
        Ammo.onTargetHit += IncreaseScore;
    }

    private void Start() {
        LoadHighscores();
    }

    void IncreaseScore() {
        score += 10;
        SetScoreText();
    }

    void ResetScore() {
        score = 0;
        SetScoreText();
    }

    void SetScoreText() {
        scoreText.text = $"Score: {score}";
    }

    void SetHighscoreText() {
        highscoreText.text = $"{highscores[2]}\n{highscores[1]}\n{highscores[0]}";
    }

    void LoadHighscores() {
        
        // Loop through the top three highscores.
        for (int i = 0; i < 3; i++) {
            // Lood scores from local storage.
            highscores[i] = PlayerPrefs.GetInt($"Highscore{i}", 0);
        }
    }

    void UpdateHighscores() {
        // Check if new score is a highscore.
        for (int i = highscores.Length - 1; i >= 0 ; i--) {

            

            // If new score is greater than given highscore...
            if (score > highscores[i]) {

                // Shift current highscores to left.
                for (int j = 0; j < i; j++) {    
                    highscores[j] = highscores[j + 1];
                }

                highscores[i] = score;
                break;
            }
        }
        SetHighscoreText();
        
        // Save updated highscores to local storage.
        SaveHighscores();
    }

    void SaveHighscores() {
        
        // Loop through the top three highscores.
        for (int i = 0; i < 3; i++) {
            // Save scores to local storage.
            PlayerPrefs.SetInt($"Highscore{i}", highscores[i]);
        }

    }

    public void ClearHighscores() {
        for (int i = 0; i < highscores.Length; i++) {
            highscores[i] = 0;
        }
        SetHighscoreText();
        SaveHighscores();
    }

    private void OnDisable() {
        GameManager.onStartGame -= ResetScore;
        GameManager.onGameOver -= UpdateHighscores;
        Ammo.onTargetHit -= IncreaseScore;
    }
}

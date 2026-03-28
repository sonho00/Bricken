using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject retryPanel;

    [SerializeField] private TextMeshProUGUI[] rankTexts;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    private void Start()
    {
        SetHighScore(PlayerPrefs.GetInt("HighScore", 0));
        SetScore(0);
        SetGold(0);
    }

    public void SetHighScore(int highScore)
    {
        highScoreText.text = "High Score: " + highScore;
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void SetGold(int gold)
    {
        goldText.text = "Gold: " + gold;
    }

    public async void ShowLeaderboard(bool show)
    {
        leaderboardPanel.SetActive(show);
        if (!show) return;

        List<RankData> data = await networkManager.RequestLeaderboard();
        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < data.Count)
            {
                var (name, score) = data[i];
                rankTexts[i].text = $"{i + 1}. {name} - {score}";
            }
            else
            {
                rankTexts[i].text = $"{i + 1}. ---";
            }
        }
    }

    public void ShowPausePanel(bool show)
    {
        pausePanel.SetActive(show);
    }

    public void ShowRetryPanel(bool show)
    {
        if (show)
        {
            gameOverScoreText.text = $"Your Score: {gameManager.score}";
        }
        retryPanel.SetActive(show);
    }
}

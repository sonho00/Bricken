using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject retryPanel;

    [SerializeField] private TextMeshProUGUI[] rankTexts;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goldText;

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

    public void ShowPausePanel(bool show)
    {
        pausePanel.SetActive(show);
    }

    public void ShowRetryPanel(bool show)
    {
        retryPanel.SetActive(show);
    }
}

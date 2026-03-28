using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;

public class RetryButton : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private TMP_InputField nameInput;

    public void OnRetryButtonClicked()
    {
        string playerName = nameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", playerName);
        }
        Debug.Log($"Posting score for {playerName} with score {gameManager.score}");
        networkManager.PostScore(playerName, gameManager.score).Forget();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

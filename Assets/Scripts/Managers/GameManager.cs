using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BrickManager brickManager;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject retryMenu;

    [SerializeField] private TextMeshProUGUI highRecordText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _score = 0;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = "Score: " + _score;
        }
    }

    [SerializeField] private TextMeshProUGUI goldText;
    private int _gold = 0;
    public int gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            goldText.text = "Gold: " + _gold;
        }
    }

    public GameState gameState;

    private void Start()
    {
        NextRound();
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    public void Fire(Vector2 start, Vector2 end)
    {
        if (gameState == GameState.Ready)
        {
            if (weaponManager.Fire(start, end))
            {
                gameState = GameState.Play;
            }
            else
            {
                gameState = GameState.Ready;
            }
        }
    }

    public void NextRound()
    {
        brickManager.CreateBrick(++score);

        int highRecord = PlayerPrefs.GetInt("HighRecord", 0);
        if (score > highRecord)
        {
            PlayerPrefs.SetInt("HighRecord", score);
            highRecord = score;
        }
        highRecordText.text = "High Record: " + highRecord;

        if (brickManager.IsGameOver())
        {
            gameState = GameState.GameOver;
            retryMenu.SetActive(true);
            scoreText.text = "Score: " + score;
        }
        else
        {
            ++gold;
            gameState = GameState.Ready;
            weaponManager.ReselectWeapon();
        }
    }

    public void Pause()
    {
        gameState = GameState.Pause;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        gameState = GameState.Ready;
        pauseMenu.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BrickManager brickManager;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private UIManager uiManager;

    private int _score = 0;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            uiManager.SetScore(_score);
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
            uiManager.SetGold(_gold);
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

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }
        uiManager.SetHighScore(highScore);

        if (brickManager.IsGameOver())
        {
            gameState = GameState.GameOver;
            uiManager.ShowRetryPanel(true);
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
    }

    public void Resume()
    {
        gameState = GameState.Ready;
    }
}

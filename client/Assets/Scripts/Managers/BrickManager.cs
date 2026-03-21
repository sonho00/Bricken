using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject brickPrefab;
    public const float brickSize = 120f;
    private readonly Vector2 offset = new Vector2(60f, 360f);
    private const int numBricksX = 9;
    private const int numBricksY = 12;
    private Brick[,] bricks;

    private void Start()
    {
        bricks = new Brick[numBricksX, numBricksY];
        for (int i = 0; i < numBricksX; ++i)
        {
            for (int j = 0; j < numBricksY; ++j)
            {
                Vector2 position = offset + brickSize * new Vector2(i, j);
                GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
                bricks[i, j] = brick.GetComponentInChildren<Brick>();
            }
        }
    }

    public bool IsGameOver()
    {
        for (int i = 0; i < numBricksX; ++i)
        {
            if (bricks[i, 0].hp > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void CreateBrick(int hp)
    {
        // Shift brick HP down
        for (int i = 0; i < numBricksX; ++i)
        {
            for (int j = 1; j < numBricksY; ++j)
            {
                bricks[i, j - 1].hp = bricks[i, j].hp;
            }
        }

        // Assign new HP values to the top row bricks
        // Avoid empty or full line
        int random = Random.Range(1, (1 << numBricksX) - 1);
        for (int i = 0, bit = 1 << (numBricksX - 1); i < numBricksX; ++i, bit >>= 1)
        {
            bricks[i, numBricksY - 1].hp = (random & bit) != 0 ? hp : 0;
        }
    }

    public Brick GetClosestBrick(Vector2 position)
    {
        position -= offset;
        int i = Mathf.RoundToInt(position.x / brickSize);
        int j = Mathf.RoundToInt(position.y / brickSize);
        i = Mathf.Clamp(i, 0, numBricksX - 1);
        j = Mathf.Clamp(j, 0, numBricksY - 1);
        return bricks[i, j];
    }
}

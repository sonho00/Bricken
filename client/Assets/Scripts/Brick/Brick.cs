using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int _hp;
    public int hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            hpText.text = _hp.ToString();
            spriteRenderer.color = Color.Lerp(Color.white, Color.cyan, (float)_hp / gameManager.score);

            hitBox.SetActive(_hp > 0);
            hpText.enabled = _hp > 0;
        }
    }

    private void Awake()
    {
        hpText.rectTransform.position = transform.position;
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            breakEffect.SetActive(true);
        }
    }
}

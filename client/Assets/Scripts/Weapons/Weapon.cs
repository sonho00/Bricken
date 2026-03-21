using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameManager gameManager;
    private WeaponManager weaponManager;
    [SerializeField] private GameObject input;
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject icon;

    public WeaponType weaponType;
    public int level;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        weaponManager = FindFirstObjectByType<WeaponManager>();
    }

    public void SelectWeapon()
    {
        weaponManager.SelectWeapon(weaponType);
        if (gameManager.gameState == GameState.Ready)
        {
            input.SetActive(true);
        }
        ShowIcon(new Vector2(300f, 150f));
    }

    public void DeselectWeapon()
    {
        input.SetActive(false);
    }

    public bool Fire(Vector2 start, Vector2 end)
    {
        if (level > 0)
        {
            input.SetActive(false);
            projectile.gameObject.SetActive(true);
            projectile.damage = damage(level);
            projectile.Fire(start, end);
            return true;
        }
        return false;
    }

    public void ShowIcon(Vector2 position)
    {
        icon.SetActive(true);
        icon.transform.position = position;
    }

    public void HideIcon()
    {
        icon.SetActive(false);
    }

    public int damage(int level)
    {
        return 1 << (level - 1);
    }

    public int cost(int level)
    {
        return 1 << level;
    }
}

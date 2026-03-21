using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject buttons;
    [SerializeField] private TextMeshProUGUI upgradeText;
    public Weapon[] weapons;
    public Weapon weapon;

    private void Awake()
    {
        weapons = new Weapon[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            weapon = transform.GetChild(i).GetComponent<Weapon>();
            weapons[(int)weapon.weaponType] = weapon;
        }
        weapon = null;
    }

    private void Start()
    {
        ChangeWeapon();
    }

    public void ChangeWeapon()
    {
        if (weapon)
        {
            weapon.DeselectWeapon();
            weapon = null;
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            float iconPositionX = 1080f / (2 * weapons.Length) * (2 * i + 1);
            weapons[i].ShowIcon(new Vector2(iconPositionX, 150f));
        }
        buttons.SetActive(false);
    }

    public bool Fire(Vector2 start, Vector2 end)
    {
        return weapon.Fire(start, end);
    }

    public void ReselectWeapon()
    {
        if (weapon != null)
        {
            weapon.SelectWeapon();
        }
    }

    public void UpgradeButton()
    {
        if (gameManager.gold >= weapon.cost(weapon.level))
        {
            gameManager.gold -= weapon.cost(weapon.level++);
            UpdateText();
        }
    }

    public void SelectWeapon(WeaponType weaponType)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].HideIcon();
        }
        weapon = weapons[(int)weaponType];
        buttons.SetActive(true);
        UpdateText();
    }

    private void UpdateText()
    {
        int level = weapon.level;
        if (level > 0)
        {
            upgradeText.text = $"Level: {level}\nDamage: {weapon.damage(level)}\nCost: {weapon.cost(level)} Gold";
        }
        else
        {
            upgradeText.text = "Unlock\nCost: 1 Gold";
        }
    }
}

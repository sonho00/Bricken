using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Brick brick;

    public void TakeDamage(int damage)
    {
        brick.TakeDamage(damage);
    }
}

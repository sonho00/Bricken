using UnityEngine;

public class Linear : Projectile
{
    private const float speed = 1000f;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Fire(Vector2 start, Vector2 end)
    {
        transform.position = start;
        transform.right = (end - start).normalized;
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Hitbox hitbox))
        {
            hitbox.TakeDamage(damage);
            hitSound.Play();
        }
    }
}

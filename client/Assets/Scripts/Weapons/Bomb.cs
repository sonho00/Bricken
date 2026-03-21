using System.Collections;
using UnityEngine;

public class Bomb : Projectile
{
    private const float explosionDelay = 0.5f;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = true;
    }

    public override void Fire(Vector2 start, Vector2 end)
    {
        transform.position = end;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);
        spriteRenderer.enabled = false;

        boxCollider.enabled = true;
        boxCollider.size = Vector2.one;
        for (; damage > 0; damage >>= 1)
        {
            hitSound.Play();
            yield return new WaitForSeconds(explosionDelay);
            boxCollider.size += 2 * Vector2.one;
        }
        
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Hitbox hitbox))
        {
            hitbox.TakeDamage(damage);
        }
    }
}

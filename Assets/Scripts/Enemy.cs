using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float movementSpeed;
    [SerializeField] private uint maxHealth;
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] Rigidbody2D rb2d;

    public uint contactDamage;
    public Health health { get; private set; }

    ~Enemy () {
        this.health.OnDeath -= Die;
    }

    private void Start () {
        this.health = new Health(this.maxHealth);
        this.health.OnDeath += Die;
        MoveTo(this.movementDirection * this.movementSpeed);
    }

    private void Die () {
        Destroy(this.gameObject);
    }

    public void DealWithCollision () {
        this.movementDirection *= -1;
        MoveTo(this.movementDirection * this.movementSpeed);
    }

    private void MoveTo (Vector2 direction) {
        this.rb2d.linearVelocity = direction;
        Vector3 scale = this.transform.localScale;
        scale.x = direction.x >= 0f ? 1 : -1;
        this.transform.localScale = scale;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class Projectile : GenericProjectile {
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float projectileSpeed;
    [SerializeField] uint damage;
    [SerializeField] float timeToLive;

    private bool damaged = false;

    public override void Fire(Vector2 direction) {
        this.rb2d.linearVelocity = direction.normalized * this.projectileSpeed;
        Destroy(this.gameObject, this.timeToLive);
    }

    private void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag("Ground")) {
            Destroy(this.gameObject);
            return;
        }
        if (collider.CompareTag("Enemy") && !this.damaged) {
            this.damaged = true;
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.health.TakeDamage(this.damage);
            Destroy(this.gameObject);
            return;
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float projectileSpeed;
    [SerializeField] uint damage;

    public void GiveVelocity (Vector2 v) {
        this.rb2d.linearVelocity = v.normalized * this.projectileSpeed;
    }

    private void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag("Ground")) {
            Destroy(this.gameObject);
            return;
        }
        if (collider.CompareTag("Enemy")) {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.health.TakeDamage(damage);
            Destroy(this.gameObject);
            return;
        }
    }
}

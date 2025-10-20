using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BoomerProjectile : GenericProjectile{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float projectileSpeed;
    [SerializeField] uint damage, uppedDamage;
    [SerializeField] float time;
    [SerializeField] float timeToLive;
    [SerializeField] Vector3 spin;

    private bool damaged = false;

    public override void Fire(Vector2 direction) {
        StartCoroutine(Trajectory(direction.normalized));
        StartCoroutine(Spin());
    }

    private IEnumerator Trajectory (Vector2 direction) {
        float timer = this.time;
        while (timer > 0f) {
            this.rb2d.linearVelocity = this.projectileSpeed * timer * direction;
            timer -= Time.deltaTime;
            yield return null;
        }

        this.damage = this.uppedDamage;

        timer = 0f;
        direction *= -1;
        while (timer < this.timeToLive) {
            this.rb2d.linearVelocity = this.projectileSpeed * timer * direction;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private IEnumerator Spin () {
        while (true) {
            this.transform.Rotate(spin);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
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

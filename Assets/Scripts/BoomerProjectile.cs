using System.Collections;
using UnityEngine;

public class BoomerProjectile : GenericProjectile {
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float projectileSpeed;

    [SerializeField] private uint baseDamage;
    [SerializeField] private uint returnDamage;

    [SerializeField] private float travelTime;
    [SerializeField] private float returnTime;

    [SerializeField] private Vector3 spin;

    private bool damaged = false;
    private uint currentDamage;

    private void Awake() {
        currentDamage = baseDamage;
    }

    public override void Fire(Vector2 direction) {
        StartCoroutine(Trajectory(direction.normalized));
        StartCoroutine(Spin());
    }

    private IEnumerator Trajectory(Vector2 direction) {
        float timer = travelTime;

        // ida
        while (timer > 0f) {
            if (rb2d != null)
                rb2d.linearVelocity = projectileSpeed * timer * direction;

            timer -= Time.deltaTime;
            yield return null;
        }

        // aumenta dano na volta
        currentDamage = returnDamage;

        // volta
        timer = 0f;
        direction *= -1;

        while (timer < returnTime) {
            if (rb2d != null)
                rb2d.linearVelocity = projectileSpeed * timer * direction;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator Spin() {
        while (true) {
            transform.Rotate(spin);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Ground")) {
            Destroy(gameObject);
            return;
        }

        if (collider.CompareTag("Enemy") && !damaged) {
            damaged = true;

            Enemy enemy = collider.GetComponentInParent<Enemy>();

            if (enemy != null && enemy.health != null) {
                enemy.health.TakeDamage(currentDamage);
            } else {
                Debug.LogWarning("Enemy ou Health não encontrado em: " + collider.name);
            }

            Destroy(gameObject);
        }
    }
}
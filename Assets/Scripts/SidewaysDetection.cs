using UnityEngine;

public class SidewaysDetection : MonoBehaviour {
    private Enemy papa;

    private void Awake() {
        papa = GetComponentInParent<Enemy>(); // 👈 pega automaticamente
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            if (papa != null)
                papa.DealWithCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Stopper")) {
            if (papa != null)
                papa.DealWithCollision();
        }
    }
}
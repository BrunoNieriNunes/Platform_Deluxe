using UnityEngine;

public class SidewaysDetection : MonoBehaviour {
    [SerializeField] private Enemy papa;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            this.papa.DealWithCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Stopper")) {
            this.papa.DealWithCollision();
        }
    }
}

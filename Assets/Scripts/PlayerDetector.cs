using UnityEngine;

public class PlayerDetector : MonoBehaviour {
    [SerializeField] private Turret papa;

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            this.InitiateLaser();
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            this.InitiateLaser();
        }
    }

    private void InitiateLaser() {
        if (!papa.isFiring) {
            StartCoroutine(papa.FireLaser());
        }
    }
}

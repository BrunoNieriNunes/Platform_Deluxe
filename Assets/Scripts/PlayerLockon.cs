using UnityEngine;

public class PlayerLockon : MonoBehaviour {
    [SerializeField] private Stronk papa;

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            this.InitiatePunch();
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            this.InitiatePunch();
        }
    }

    private void InitiatePunch() {
        if (!papa.isFiring) {
            StartCoroutine(papa.GigaPunch());
        }
    }
}

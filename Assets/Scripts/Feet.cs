using UnityEngine;

public class Feet : MonoBehaviour {
    [SerializeField] MainCharacter character;

    private void OnTriggerEnter2D(Collider2D collider) {
        this.character.MakeAirborne(false);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        this.character.MakeAirborne(true);
    }

    private void OnTriggerStay2D(Collider2D collider) {
        this.character.MakeAirborne(false);
    }
}

using UnityEngine;
using System.Collections;

public class Stronk : Enemy {
    [SerializeField] private GameObject punchBox;
    public bool isFiring { get; private set; }
    [SerializeField] private float windupTime = 0.25f;
    [SerializeField] private float punchDurationTime = 0.25f;
    [SerializeField] private float extraDelay = 0.5f;

    private void Start() {
        this.isFiring = false;
        this.punchBox.SetActive(false);
        this.health = new Health(this.maxHealth);
        this.health.OnDeath += Die;
        MoveTo(this.movementDirection * this.movementSpeed);
    }

    public IEnumerator GigaPunch() {
        this.rb2d.linearVelocity = Vector3.zero;
        this.isFiring = true;
        //windup
        yield return new WaitForSeconds(windupTime);
        //fire
        this.punchBox.SetActive(true);
        yield return new WaitForSeconds(punchDurationTime);
        //end
        this.punchBox.SetActive(false);
        //wait a bit
        yield return new WaitForSeconds(extraDelay);
        this.isFiring = false;
        this.MoveTo(this.movementDirection * this.movementSpeed);
    }
}

using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] private uint maxHealth;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private GameObject laserBox;
    [SerializeField] private float windupTime = 0.25f;
    [SerializeField] private float laserDurationTime = 0.25f;
    [SerializeField] private float extraDelay = 0.5f;
    public bool isFiring { get; private set; }

    public uint contactDamage;
    public Health health { get; private set; }

    ~Turret() {
        if (this.health != null) {
            this.health.OnDeath -= Die;
        }
    }

    private void Start() {
        this.health = new Health(this.maxHealth);
        this.health.OnDeath += Die;
        this.laserBox.SetActive(false);
    }

    private void Die() {
        Destroy(this.gameObject);
    }

    public IEnumerator FireLaser() {
        this.isFiring = true;
        //windup
        yield return new WaitForSeconds(windupTime);
        //fire
        this.laserBox.SetActive(true);
        yield return new WaitForSeconds(laserDurationTime);
        //end
        this.laserBox.SetActive(false);
        //wait a bit
        yield return new WaitForSeconds(extraDelay);
        this.isFiring = false;
    }
}
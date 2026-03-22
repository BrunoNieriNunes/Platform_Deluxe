using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected uint maxHealth;
    [SerializeField] protected Vector2 movementDirection;
    [SerializeField] protected Rigidbody2D rb2d;
    [SerializeField] protected Animator animator;

    public uint contactDamage;
    public Health health { get; protected set; }

    // Parâmetro para controlar velocidade da animação
    private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");

    ~Enemy() {
        if (this.health != null) {
            this.health.OnDeath -= Die;
        }
    }

    private void Start() {
        this.health = new Health(this.maxHealth);
        this.health.OnDeath += Die;
        MoveTo(this.movementDirection * this.movementSpeed);
    }

    private void Update() {
        // Ajustar velocidade da animação baseado na velocidade real
        if (animator != null) {
            float currentSpeed = rb2d.linearVelocity.magnitude;
            float normalizedSpeed = currentSpeed / movementSpeed;
            float speedMultiplier = 0.01f; // Experimente valores entre 0.1 e 2.0
            animator.SetFloat(AnimationSpeed, normalizedSpeed * speedMultiplier);
        }
    }

    protected void Die() {
        Destroy(this.gameObject);
    }

    public void DealWithCollision() {
        this.movementDirection *= -1;
        MoveTo(this.movementDirection * this.movementSpeed);
    }

    protected void MoveTo(Vector2 direction) {
        this.rb2d.linearVelocity = direction;
        Vector3 scale = this.transform.localScale;
        scale.x = direction.x >= 0f ? 1 : -1;
        this.transform.localScale = scale;
    }
}
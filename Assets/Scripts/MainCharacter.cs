using UnityEngine;
using UnityEngine.InputSystem;


public class MainCharacter : MonoBehaviour {
    [SerializeField] private uint maxHealth;

    [SerializeField] private float movementSpeed;

    [SerializeField] private float attackDelay;

    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 1f)] private float jumpCut;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBuffer;

    [SerializeField] private GenericProjectile[] projectiles;
    [SerializeField] private int selectedProjectile;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer sprite;

    public Health health { get; private set; }

    private float attackTimer;

    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool airborne;
    private int[] facingDirection;
    private bool verticalDirectionOnly;

    private readonly float tolerance = 0.05f;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    ~MainCharacter() {
        this.health.OnDeath -= Die;
    }

    private void Start () {
        this.health = new Health(this.maxHealth);
        this.health.OnDeath += Die;
        this.attackTimer = 0f;
        this.coyoteTimer = 0f;
        this.jumpBufferTimer = 0f;
        this.airborne = false;
        this.facingDirection = new int[] {1, 0};
        this.verticalDirectionOnly = false;
        this.moveAction = InputSystem.actions.FindAction("Move");
        this.jumpAction = InputSystem.actions.FindAction("Jump");
        this.attackAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update () {
        //ANDAR

        Vector2 moveValue = this.moveAction.ReadValue<Vector2>();
        Walk(moveValue * this.movementSpeed, this.rb2d);

        //DIREÇÃO

        if (moveValue.y > this.tolerance) {
            this.facingDirection[1] = 1;
        }
        else if (moveValue.y < -this.tolerance) {
            this.facingDirection[1] = -1;
        }
        else {
            this.facingDirection[1] = 0;
        }

        this.verticalDirectionOnly = false;
        if (moveValue.x > this.tolerance) {
            this.facingDirection[0] = 1;
        }
        else if (moveValue.x < -this.tolerance) {
            this.facingDirection[0] = -1;
        }
        else if (this.facingDirection[1] != 0) {
            this.verticalDirectionOnly = true;
        }

        if (this.facingDirection[0] != 0) {
            Vector3 scale = this.transform.localScale;
            scale.x = this.facingDirection[0];
            this.transform.localScale = scale;
        }

        //PULO

        /*
        if (this.jumpAction.WasPerformedThisFrame() && this.airborne) {
            this.jumpBufferTimer = this.jumpBuffer;
        }
        if (!this.airborne && (this.jumpAction.WasPerformedThisFrame() || this.jumpBufferTimer > 0f) || this.jumpAction.WasPerformedThisFrame() && coyoteTimer > 0f) {
            Jump(this.jumpForce, this.rb2d);
        }
        if (this.jumpBufferTimer > 0f) {
            this.jumpBufferTimer -= Time.deltaTime;
        }
        if (this.coyoteTimer > 0f) {
            this.coyoteTimer -= Time.deltaTime;
        }
        */

        if (!this.airborne && this.jumpAction.WasPerformedThisFrame()) {
            this.Jump(this.jumpForce, this.rb2d);
        }

        //ATAQUE

        if (this.attackTimer <= 0f) {
            if (this.attackAction.WasPressedThisFrame()) {
                Attack(this.projectiles[this.selectedProjectile], new Vector2(this.verticalDirectionOnly ? 0 : this.facingDirection[0], this.facingDirection[1]));
                this.attackTimer = this.attackDelay;
            }
        }
        else {
            this.attackTimer -= Time.deltaTime;
        }
    }

    private void Walk (Vector2 moveValue, Rigidbody2D rb2d) {
        Vector2 movement = rb2d.linearVelocity;
        movement.x = moveValue.x;
        rb2d.linearVelocity = movement;
    }

    private void Jump (float jumpForce, Rigidbody2D rb2d) {
        this.airborne = true;
        /*
        this.coyoteTimer = 0f;
        this.jumpBufferTimer = 0f;
        */
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Attack (GenericProjectile projectile, Vector2 direction) {
        GenericProjectile instantiated = Instantiate(projectile, this.transform.position + Vector3.back, Quaternion.identity);
        instantiated.Fire(direction);
    }

    public void MakeAirborne (bool airborne) {
        this.airborne = airborne;
        /*
        this.coyoteTimer = this.coyoteTime;
        */
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            Enemy enemy = collider.GetComponent<Enemy>();
            this.health.TakeDamage(enemy.contactDamage);
        }
    }
    
    /*
    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            this.airborne = false;
        }
    }
    */

    private void Die () {
        Destroy(this.gameObject);
    }
}

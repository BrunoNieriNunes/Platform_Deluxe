using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour {
    [Header("Status")]
    [SerializeField] private uint maxHealth;

    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 1f)] private float jumpCut;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBuffer;

    [Header("Combat")]
    [SerializeField] private float attackDelay;
    [SerializeField] private GenericProjectile[] projectiles;
    [SerializeField] private int selectedProjectile;
    
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer sprite;

    // --- ADIÇÃO: Variáveis de Animação ---
    private Animator animator;
    private float shootingAnimTimer; 
    private readonly float shootingAnimDuration = 0.2f; // Tempo que a animação de tiro fica ativa
    // -------------------------------------

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
        if (this.health != null)
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

        // --- CORREÇÃO: Busca o Animator nos filhos (na Sprite) ---
        this.animator = GetComponentInChildren<Animator>();
    }

    private void Update () {
        // 1. INPUT DE MOVIMENTO
        Vector2 moveValue = this.moveAction.ReadValue<Vector2>();
        Walk(moveValue * this.movementSpeed, this.rb2d);

        // 2. CÁLCULO DE DIREÇÃO (Atualiza facingDirection)
        if (moveValue.y > this.tolerance) {
            this.facingDirection[1] = 1; // Cima
        }
        else if (moveValue.y < -this.tolerance) {
            this.facingDirection[1] = -1; // Baixo
        }
        else {
            this.facingDirection[1] = 0; // Neutro
        }

        this.verticalDirectionOnly = false;
        if (moveValue.x > this.tolerance) {
            this.facingDirection[0] = 1; // Direita
        }
        else if (moveValue.x < -this.tolerance) {
            this.facingDirection[0] = -1; // Esquerda
        }
        else if (this.facingDirection[1] != 0) {
            this.verticalDirectionOnly = true;
        }

        // Vira o personagem horizontalmente
        if (this.facingDirection[0] != 0) {
            Vector3 scale = this.transform.localScale;
            scale.x = this.facingDirection[0];
            this.transform.localScale = scale;
        }

        // 3. ATUALIZAÇÃO DO ANIMATOR
        if (this.animator != null)
        {
            // Define Velocidade (Run vs Idle)
            float horizontalSpeed = Mathf.Abs(this.rb2d.linearVelocity.x);
            this.animator.SetFloat("Speed", horizontalSpeed);

            // Define Direção Vertical (1 = Cima, 0 = Frente, -1 = Baixo)
            this.animator.SetInteger("Vertical", this.facingDirection[1]);

            // Define se está Atirando (Timer)
            if (this.shootingAnimTimer > 0)
            {
                this.shootingAnimTimer -= Time.deltaTime;
                this.animator.SetBool("IsShooting", true);
            }
            else
            {
                this.animator.SetBool("IsShooting", false);
            }
        }

        // 4. PULO
        if (!this.airborne && this.jumpAction.WasPerformedThisFrame()) {
            this.Jump(this.jumpForce, this.rb2d);
        }

        // 5. ATAQUE
        if (this.attackTimer <= 0f) {
            if (this.attackAction.WasPressedThisFrame()) {
                // Calcula a direção do tiro
                Vector2 shootDir = new Vector2(this.verticalDirectionOnly ? 0 : this.facingDirection[0], this.facingDirection[1]);
                
                Attack(this.projectiles[this.selectedProjectile], shootDir);
                
                this.attackTimer = this.attackDelay;

                // --- Ativa a flag de animação de tiro ---
                this.shootingAnimTimer = this.shootingAnimDuration;
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
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Attack (GenericProjectile projectile, Vector2 direction) {
        // Cria o projétil um pouco atrás para não colidir imediatamente com o player se necessário, ou ajuste o offset
        GenericProjectile instantiated = Instantiate(projectile, this.transform.position + Vector3.back, Quaternion.identity);
        instantiated.Fire(direction);
    }

    public void MakeAirborne (bool airborne) {
        this.airborne = airborne;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            // Verificação de segurança caso o inimigo não tenha o componente
            if (collider.TryGetComponent<Enemy>(out var enemy)) {
                this.health.TakeDamage(enemy.contactDamage);
            }
            else {
                if (collider.TryGetComponent<Turret>(out var turret)) {
                    this.health.TakeDamage(turret.contactDamage);
                }
            }
        }
        else if (collider.CompareTag("Laser")) {
            if (collider.TryGetComponent<Laser>(out var laser)) {
                this.health.TakeDamage(laser.contactDamage);
            }
        }
    }
    
    private void Die () {
        Destroy(this.gameObject);
    }
}
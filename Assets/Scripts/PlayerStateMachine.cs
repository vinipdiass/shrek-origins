using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour
{
    private enum PlayerState { Idle, Walking, Punching, Roaring, Farting }
    private bool hasPunch = false;
    private bool hasRoar = false;
    private bool hasFart = false;
    private PlayerState currentState;
    public int experiencePoints;
    public float experiencePointsRequired;
    private Bounds playerBounds;
    private bool isPunchingCoroutineRunning = false;
    private bool isRoaringCoroutineRunning = false;
    private bool isFartingCoroutineRunning = false;
    public float speed = 5f;
    private CharacterController characterController;
    private Punch soco;    // For punch attacks
    private Roar rugido;   // For roar attacks
    private Fart peido;    // For fart attacks
    public Animator animator;
    public int recovery;

    // Origin points for attacks (not needed for Fart anymore)
    public Transform pontoOrigem;
    public Transform pontoOrigemRugido;

    // Health variables
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    // Reference to SpriteRenderer
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Atributtes atributos;

    private void Start()
    {
        recovery = 0;
        currentHealth = maxHealth;
        experiencePoints = 0;
        experiencePointsRequired = 5f;

        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator não encontrado no GameObject");
        characterController = GetComponent<CharacterController>();

        // Initialize abilities
        soco = GetComponent<Punch>();
        rugido = GetComponent<Roar>();
        peido = GetComponent<Fart>();

        atributos = GetComponent<Atributtes>();

        // Get the SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.LogError("SpriteRenderer não encontrado no GameObject");

        // Store the original color
        originalColor = spriteRenderer.color;

        // Activate abilities based on player preferences
        string playerAbility = PlayerPrefs.GetString("PlayerAbility", "None");
        if (playerAbility == "Punch")
        {
            ActivatePunch();
        }
        else if (playerAbility == "Roar")
        {
            ActivateRoar();
        }
        else if (playerAbility == "Fart")
        {
            ActivateFart();
        }

        ChangeState(PlayerState.Idle); // Initial state

        float playerWidth = characterController.bounds.extents.x * 0.3f;  // Slightly reduce margin
        float playerHeight = characterController.bounds.extents.y * 0.3f; // Slightly reduce margin

        playerBounds = new Bounds();
        playerBounds.SetMinMax(
            new Vector3(Globals.WorldBounds.min.x + playerWidth, Globals.WorldBounds.min.y + playerHeight, 0.0f),
            new Vector3(Globals.WorldBounds.max.x - playerWidth, Globals.WorldBounds.max.y - playerHeight, 0.0f)
        );

        StartCoroutine(RegenerateHealth());
    }

    private void Update()
    {
        HandleMovement();

        // Ability evolution
        if (Input.GetKeyDown(KeyCode.I) && experiencePoints >= experiencePointsRequired)
        {
            soco.evolute();
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.O) && experiencePoints >= experiencePointsRequired)
        {
            rugido.evolute();
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.P) && experiencePoints >= experiencePointsRequired)
        {
            peido.Evolute();
            experiencePoints = 0;
        }

        // Ability activation
        if (Input.GetKeyDown(KeyCode.K) && experiencePoints >= experiencePointsRequired)
        {
            hasPunch = true;
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.L) && experiencePoints >= experiencePointsRequired)
        {
            hasRoar = true;
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.J) && experiencePoints >= experiencePointsRequired)
        {
            hasFart = true;
            experiencePoints = 0;
        }

        // Attribute evolution
        if (Input.GetKeyDown(KeyCode.Y) && experiencePoints >= experiencePointsRequired)
        {
            if (atributos.getLevelDamage() == 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                atributos.increaseLevelDamage();
                soco.addAtributeAttack();
                rugido.addAtributeAttack();
                peido.AddAttributeAttack();
                experiencePoints = 0;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.T) && experiencePoints >= experiencePointsRequired)
        {
            if (atributos.getLevelMaxLife() == 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                this.maxHealth += 50;
                this.currentHealth += 50;
                atributos.increaseLevelMaxLife();
                experiencePoints = 0;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R) && experiencePoints >= experiencePointsRequired)
        {
            if (atributos.getLevelRecovery() == 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                this.recovery++;
                atributos.increaseLevelRecovery();
                experiencePoints = 0;
            }
           
        }
        if (Input.GetKeyDown(KeyCode.E) && experiencePoints >= experiencePointsRequired)
        {
            if (atributos.getLevelCooldown() == 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                atributos.increaseLevelCooldown();
                soco.addAtributeCooldownReduction();
                rugido.addAtributeCooldownReduction();
                // Implement cooldown reduction in Fart if needed
                experiencePoints = 0;
            }
            
        }

        switch (currentState)
        {
            case PlayerState.Idle:
                UpdateIdleState();
                break;
            case PlayerState.Walking:
                UpdateWalkingState();
                break;
            case PlayerState.Punching:
                if (!isPunchingCoroutineRunning)
                {
                    StartCoroutine(UpdatePunchState());
                }
                break;
            case PlayerState.Roaring:
                if (!isRoaringCoroutineRunning)
                {
                    StartCoroutine(UpdateRoarState());
                }
                break;
            case PlayerState.Farting:
                if (!isFartingCoroutineRunning)
                {
                    StartCoroutine(UpdateFartState());
                }
                break;
        }

        // Cooldown timers
        soco.IncreaseTimer();
        rugido.IncreaseTimer();
        peido.IncreaseTimer();
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Wait 2 seconds

            if (currentHealth < maxHealth)
            {
                currentHealth += recovery;
                if (currentHealth > maxHealth) // Ensure currentHealth doesn't exceed maxHealth
                {
                    currentHealth = maxHealth;
                }
            }
        }
    }

    public void ActivateRoar()
    {
        this.hasRoar = true;
    }

    public void ActivatePunch()
    {
        this.hasPunch = true;
    }

    public void ActivateFart()
    {
        this.hasFart = true;
    }

    public void AddExperience(int amount)
    {
        experiencePoints += amount;
        Debug.Log("Current XP: " + experiencePoints);
    }

    private void ChangeState(PlayerState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
    }

    private void EnterState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.SetInteger("State", 0);
                break;
            case PlayerState.Walking:
                animator.SetInteger("State", 1);
                break;
            case PlayerState.Punching:
                animator.SetInteger("State", 2);
                soco.Activate(1);
                soco.ActivateProjectile2(0);
                soco.ActivateProjectile(0);
                break;
            case PlayerState.Roaring:
                animator.SetInteger("State", 3);
                rugido.ActivateRoar(1);
                rugido.ActivateProjectile(0);
                break;
            case PlayerState.Farting:
                animator.SetInteger("State", 4);
                peido.ActivateFart(1);
                break;
        }
    }

    private void ExitState(PlayerState state)
    {
        if (state == PlayerState.Punching)
        {
            soco.Activate(0);
            soco.ActivateProjectile2(0);
            soco.ActivateProjectile(0);
            soco.resetTimer();
            isPunchingCoroutineRunning = false;
        }
        else if (state == PlayerState.Roaring)
        {
            rugido.ActivateRoar(0);
            rugido.ActivateProjectile(0);
            rugido.resetTimer();
            isRoaringCoroutineRunning = false;
        }
        else if (state == PlayerState.Farting)
        {
            peido.ActivateFart(0);
            peido.ResetTimer();
            isFartingCoroutineRunning = false;
        }
    }

    private void UpdateIdleState()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        else if (!peido.IsInCooldown() && hasFart)
        {
            ChangeState(PlayerState.Farting);
        }
        else if (!rugido.IsInCooldown() && hasRoar)
        {
            ChangeState(PlayerState.Roaring);
        }
        else if (!soco.IsInCooldown() && hasPunch)
        {
            ChangeState(PlayerState.Punching);
        }
    }

    private void UpdateWalkingState()
    {
        if (!peido.IsInCooldown() && hasFart)
        {
            ChangeState(PlayerState.Farting);
        }
        else if (!rugido.IsInCooldown() && hasRoar)
        {
            ChangeState(PlayerState.Roaring);
        }
        else if (!soco.IsInCooldown() && hasPunch)
        {
            ChangeState(PlayerState.Punching);
        }

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized;

        if (moveHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Rotate left
        }
        else if (moveHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Rotate right
        }

        Vector3 moveDirection = new Vector3(movement.x, movement.y, 0) * speed;
        characterController.Move(moveDirection * Time.deltaTime);

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(transform.position.x, playerBounds.min.x, playerBounds.max.x),
            Mathf.Clamp(transform.position.y, playerBounds.min.y, playerBounds.max.y),
            transform.position.z
        );

        transform.position = clampedPosition;
    }

    // Function to receive damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Start the flashing coroutine
        StartCoroutine(FlashWhite());

        // Check if health reached zero
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        Debug.Log("O jogador recebeu dano! Vida atual: " + currentHealth);
    }

    private void Die()
    {
        // Implement behavior on death (e.g., play animation, disable controls)
        Debug.Log("O jogador morreu!");
    }

    private IEnumerator FlashWhite()
    {
        Debug.Log("Entrou no método de FlashWhite");
        int flashCount = 2; // Number of times the character will flash
        float flashDuration = 0.2f; // Duration of each flash

        for (int i = 0; i < flashCount; i++)
        {
            // Change color to red
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration / 2);

            // Return to original color
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration / 2);
        }

        // Ensure color is original at the end
        spriteRenderer.color = originalColor;
    }

    private IEnumerator UpdatePunchState()
    {
        if (isPunchingCoroutineRunning) yield break;

        isPunchingCoroutineRunning = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration / 2);

        if (!soco.isProjectile2Active())
        {
            soco.attack(pontoOrigem, transform.rotation);
            soco.ActivateProjectile2(1);
        }

        yield return new WaitForSeconds(animationDuration / 3);

        if (!soco.isProjectileActive())
        {
            soco.attack(pontoOrigem, transform.rotation);
            soco.ActivateProjectile(1);
        }

        soco.resetTimer();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        isPunchingCoroutineRunning = false;
    }

    private IEnumerator UpdateRoarState()
    {
        if (isRoaringCoroutineRunning) yield break;

        isRoaringCoroutineRunning = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration / 2);

        if (!rugido.isRoarActive())
        {
            rugido.RoarAttack(pontoOrigemRugido, transform.rotation);
            rugido.ActivateRoar(1);
        }

        rugido.resetTimer();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        isRoaringCoroutineRunning = false;
    }

    private IEnumerator UpdateFartState()
    {
        if (isFartingCoroutineRunning) yield break;

        isFartingCoroutineRunning = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration / 2);

        peido.FartAttack(transform.position);

        peido.ResetTimer();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        isFartingCoroutineRunning = false;
    }
}

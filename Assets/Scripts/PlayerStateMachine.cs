using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerStateMachine : MonoBehaviour
{
    private enum PlayerState { Idle, Walking, Punching, Roaring, Farting, GasAttacking, BoomerangAttacking }

    public bool hasPunch = false;
    public bool hasRoar = false;
    public bool hasFart = false;
    public bool hasGasAttack = false;
    public bool hasBeetleAttack = false;
    public bool hasBoomerangAttack = false;

    private PlayerState currentState;
    public int experiencePoints;
    public float experiencePointsRequired;
    public int money;
    private Bounds playerBounds;
    private bool isPunchingCoroutineRunning = false;
    private bool isRoaringCoroutineRunning = false;
    private bool isFartingCoroutineRunning = false;
    private bool isGasAttackingCoroutineRunning = false;
    private bool isBoomerangAttackingCoroutineRunning = false;

    public float speed = 5f;
    private CharacterController characterController;
    private Punch soco;    // Para ataques de soco
    private Roar rugido;   // Para ataques de rugido
    private Fart peido;    // Para ataques de peido
    private Onion gasAttack; // Para ataques de gás
    private BeetleAttack besouroAttack; // Para ataque do besouro
    private BoomerangAttack boomerangAttack; // Script do novo ataque

    public Animator animator;
    public int recovery;

    // Pontos de origem para ataques
    public Transform pontoOrigem;
    public Transform pontoOrigemRugido;

    // Variáveis de vida
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    // Referência ao SpriteRenderer
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Atributtes atributos;
    public int countLevel;
    public bool verificacaoLoja;
    List<int> atributosDisponiveis;

    private void Start()
    {
        recovery = 0;
        currentHealth = maxHealth;
        experiencePoints = 0;
        experiencePointsRequired = 1f;
        speed = 3f;

        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator não encontrado no GameObject");
        characterController = GetComponent<CharacterController>();

        // Inicializa habilidades
        soco = GetComponent<Punch>();
        rugido = GetComponent<Roar>();
        peido = GetComponent<Fart>();
        gasAttack = GetComponent<Onion>();
        besouroAttack = GetComponent<BeetleAttack>();
        boomerangAttack = GetComponent<BoomerangAttack>();
        countLevel = 0;


        atributos = GetComponent<Atributtes>();

        // Obtém o SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.LogError("SpriteRenderer não encontrado no GameObject");

        // Armazena a cor original
        originalColor = spriteRenderer.color;

        // Ativa habilidades com base nas preferências do jogador
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
        else if (playerAbility == "GasAttack")
        {
            ActivateGasAttack();
        }
        else if (playerAbility == "BeetleAttack")
        {
            ActivateBeetleAttack();
        }
        else if (playerAbility == "BoomerangAttack")
        {
            ActivateBoomerangAttack();
        }

        ChangeState(PlayerState.Idle); // Estado inicial

        float playerWidth = characterController.bounds.extents.x * 0.3f;  // Margem ligeiramente reduzida
        float playerHeight = characterController.bounds.extents.y * 0.3f; // Margem ligeiramente reduzida

        playerBounds = new Bounds();
        playerBounds.SetMinMax(
            new Vector3(Globals.WorldBounds.min.x + playerWidth, Globals.WorldBounds.min.y + playerHeight, 0.0f),
            new Vector3(Globals.WorldBounds.max.x - playerWidth, Globals.WorldBounds.max.y - playerHeight, 0.0f)
        );

        atributosDisponiveis = GameDataManager.instance.getAtributos();
        verificacaoLoja = true;

        StartCoroutine(RegenerateHealth());

    }

    private void Update()
    {
        HandleMovement();


        // Evolução de habilidades
        if (Input.GetKeyDown(KeyCode.I))
        {
            rugido.Evolute();
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.O) && experiencePoints >= experiencePointsRequired)
        {
            rugido.Evolute();
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.P) && experiencePoints >= experiencePointsRequired)
        {
            peido.Evolute();
            experiencePoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.U) && experiencePoints >= experiencePointsRequired)
        {
            if (besouroAttack != null)
            {
                besouroAttack.Evolute();
                experiencePoints = 0;
            }
        }

        // Ativação de habilidades
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
        if (Input.GetKeyDown(KeyCode.M) && experiencePoints >= experiencePointsRequired)
        {
            ActivateBeetleAttack();
            experiencePoints = 0;
        }

        // Evolução de atributos
        if (Input.GetKeyDown(KeyCode.Y))
        {
            IncreaseAtribute(0);
            atributos.increaseLevelMaxLife();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            IncreaseAtribute(1);
            atributos.increaseLevelRecovery();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            IncreaseAtribute(2);
            atributos.increaseLevelCooldown();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            IncreaseAtribute(3);
            atributos.increaseLevelSpeed();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            IncreaseAtribute(4);
            atributos.increaseLevelDamage();

        }


        if (verificacaoLoja == true)
        {
            aplicaLojaAtributos(0, atributosDisponiveis[0]);
            aplicaLojaAtributos(1, atributosDisponiveis[1]);
            aplicaLojaAtributos(2, atributosDisponiveis[2]);
            aplicaLojaAtributos(3, atributosDisponiveis[3]);
            aplicaLojaAtributos(4, atributosDisponiveis[4]);

            verificacaoLoja = false;
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
            case PlayerState.GasAttacking:
                if (!isGasAttackingCoroutineRunning)
                {
                    StartCoroutine(UpdateGasAttackState());
                }
                break;
            case PlayerState.BoomerangAttacking:
                if (!isBoomerangAttackingCoroutineRunning)
                {
                    StartCoroutine(UpdateBoomerangAttackState());
                }
                break;
        }

        // Timers de cooldown
        soco.IncreaseTimer();
        rugido.IncreaseTimer();
        peido.IncreaseTimer();
        gasAttack.IncreaseTimer();
        boomerangAttack.IncreaseTimer();
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Espera 2 segundos

            if (currentHealth < maxHealth)
            {
                currentHealth += recovery;
                if (currentHealth > maxHealth) // Garante que currentHealth não exceda maxHealth
                {
                    currentHealth = maxHealth;
                }
            }
        }
    }

    public float getXPRequired()
    {
        return this.experiencePointsRequired;
    }

    public void ActivateRoar()
    {
        this.hasRoar = true;
    }

    public void ActivatePunch()
    {
        this.hasPunch = true;
    }

    public void ActivateGasAttack()
    {
        this.hasGasAttack = true;
    }

    public void ActivateFart()
    {
        this.hasFart = true;
    }

    public void ActivateBeetleAttack()
    {
        this.hasBeetleAttack = true;
        if (besouroAttack != null)
        {
            besouroAttack.ActivateBeetle();
        }
    }

    public void ActivateBoomerangAttack()
    {
        this.hasBoomerangAttack = true;
    }


    public void AddExperience(int amount)
    {
        experiencePoints += amount;

    }


    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Current money: " + money);
    }

    public void AddHp(int amount)
    {
        if (currentHealth + amount >= maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;
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
            case PlayerState.GasAttacking:
                animator.SetInteger("State", 5);
                gasAttack.ActivateGasAttack(1);
                break;
            case PlayerState.BoomerangAttacking:
                animator.SetInteger("State", 6);
                boomerangAttack.ActivateBoomerangAttack(1);
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
        else if (!gasAttack.IsInCooldown() && hasGasAttack)
        {
            ChangeState(PlayerState.GasAttacking);
        }
        else if (!boomerangAttack.IsInCooldown() && hasBoomerangAttack)
        {
            ChangeState(PlayerState.BoomerangAttacking);
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
        else if (!gasAttack.IsInCooldown() && hasGasAttack)
        {
            ChangeState(PlayerState.GasAttacking);
        }
        else if (!boomerangAttack.IsInCooldown() && hasBoomerangAttack)
        {
            ChangeState(PlayerState.BoomerangAttacking);
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
            transform.eulerAngles = new Vector3(0, 180, 0); // Rotaciona para a esquerda
        }
        else if (moveHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Rotaciona para a direita
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashWhite());
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        Debug.Log("O jogador recebeu dano! Vida atual: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("O jogador morreu!");
        // Implementar lógica de morte do jogador
    }

    private IEnumerator FlashWhite()
    {
        int flashCount = 2; // Número de vezes que o personagem pisca
        float flashDuration = 0.2f; // Duração de cada flash

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration / 2);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration / 2);
        }
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

    private IEnumerator UpdateGasAttackState()
    {
        if (isGasAttackingCoroutineRunning) yield break;

        isGasAttackingCoroutineRunning = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration / 2);

        gasAttack.PerformGasAttack(transform, transform.rotation);

        gasAttack.ResetTimer();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        isGasAttackingCoroutineRunning = false;
    }

    private IEnumerator UpdateBoomerangAttackState()
    {
        if (isBoomerangAttackingCoroutineRunning) yield break;

        isBoomerangAttackingCoroutineRunning = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration / 2);

        boomerangAttack.PerformBoomerangAttack(transform, transform.rotation);

        boomerangAttack.ResetTimer();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        isBoomerangAttackingCoroutineRunning = false;
    }

    private void IncreaseAtribute(int at)
    {
        if (at == 0)
        {
            Debug.Log("Vida maxima aprimorado");

            if (atributos.getLevelMaxLife() >= 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                this.maxHealth += 70;
                this.currentHealth += 70;
                experiencePoints = 0;
            }

        }
        if (at == 1)
        {
            Debug.Log("Regen aprimorado");
            if (atributos.getLevelRecovery() >= 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                this.recovery++;
                experiencePoints = 0;
            }
        }
        if (at == 2)
        {

            if (atributos.getLevelCooldown() >= 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                Debug.Log("Cooldown aprimorado");
                soco.addAtributeCooldownReduction();
                rugido.addAtributeCooldownReduction();
                gasAttack.AddAttributeCooldownReduction();
                besouroAttack.AddAttributeCooldownReduction();
                boomerangAttack.AddAttributeCooldownReduction();
                experiencePoints = 0;
            }
        }

        if (at == 3)
        {
            Debug.Log("Velocidade aprimorada");
            if (atributos.getLevelSpeed() >= 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                speed += 0.5f;
            }
        }

        if (at == 4)
        {
            if (atributos.getLevelDamage() >= 3)
            {
                Debug.Log("Você não pode mais evoluir esse atributo");
            }
            else
            {
                Debug.Log("Dano aprimorado");
                soco.addAtributeAttack();
                rugido.addAtributeAttack();
                peido.AddAttributeAttack();
                gasAttack.AddAttributeAttack();
                besouroAttack.AddAttributeAttack();
                boomerangAttack.AddAttributeAttack();

            }
        }
    }

    public void aplicaLojaAtributos(int index, int n)
    {
        if (atributos == null)
        {
            Debug.LogError("Atributtes não inicializado!");
            return;
        }
        for (int i = 0; i < n; i++)
        {
            Debug.Log("Atributo comprado");
            IncreaseAtribute(index);
        }
    }

}

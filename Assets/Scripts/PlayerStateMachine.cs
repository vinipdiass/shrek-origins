using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//Aumentar o XP
//Resolver o BUG de XP
//Diminuir o quanto a vida cura
public class PlayerStateMachine : MonoBehaviour
{
    private enum PlayerState { Idle, Walking }




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

    // Variáveis de controle para cada ataque
    private bool isPunching = false;
    private bool isRoaring = false;
    private bool isFarting = false;
    private bool isGasAttacking = false;
    private bool isBoomerangAttacking = false;

    public bool hasPunch = false;
    public bool hasRoar = false;
    public bool hasFart = false;
    public bool hasGasAttack = false;
    public bool hasBeetleAttack = false;
    public bool hasBoomerangAttack = false;

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
    public float damageInterval = 1f; // pro dano da lava
    private float damageTimer = 0f; // dano da lava
    public AudioSource somCebola;

    private void Start()
    {
        recovery = 0;
        currentHealth = maxHealth;
        experiencePoints = 0;
        experiencePointsRequired = 30f;
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
            yield return new WaitForSeconds(1.5f); // Espera 2 segundos

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
        return experiencePointsRequired;
    }


    public float getXP()
    {
        return experiencePoints;
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
        }
    }


    private void ExitState(PlayerState state)
    {

    }


    private void CheckAndStartAttacks()
    {
        if (!peido.IsInCooldown() && hasFart && !isFarting)
        {
            animator.SetInteger("State", 4);
            StartCoroutine(UpdateFartState());
        }
        if (!rugido.IsInCooldown() && hasRoar && !isRoaring)
        {
            animator.SetInteger("State", 3);
            StartCoroutine(UpdateRoarState());
        }
        if (!soco.IsInCooldown() && hasPunch && !isPunching)
        {
            animator.SetInteger("State", 2);
            StartCoroutine(UpdatePunchState());
        }
        if (!gasAttack.IsInCooldown() && hasGasAttack && !isGasAttacking)
        {
            animator.SetInteger("State", 5);
            StartCoroutine(UpdateGasAttackState());
        }
        if (!boomerangAttack.IsInCooldown() && hasBoomerangAttack && !isBoomerangAttacking)
        {
            animator.SetInteger("State", 6);
            StartCoroutine(UpdateBoomerangAttackState());
        }
    }

    private void UpdateIdleState()
    {
        CheckAndStartAttacks();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            ChangeState(PlayerState.Walking);
        }
    }

    private void UpdateWalkingState()
    {
        CheckAndStartAttacks();

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calcula o vetor de movimento com base nos inputs do jogador
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized;

        // Cria a direção de movimento desejada
        Vector3 moveDirection = movement * speed * Time.deltaTime;

        // Verifica se o movimento está bloqueado nas direções horizontal e vertical
        if (!IsBlockedInDirection(moveDirection))
        {
            // Se não estiver bloqueado, aplica o movimento
            characterController.Move(moveDirection);
        }

        // Atualiza a rotação do jogador com base na direção horizontal
        if (moveHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Rotaciona para a esquerda
        }
        else if (moveHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Rotaciona para a direita
        }

        // Limita a posição do jogador dentro dos limites do cenário (bordas)
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(transform.position.x, playerBounds.min.x, playerBounds.max.x),
            Mathf.Clamp(transform.position.y, playerBounds.min.y, playerBounds.max.y),
            transform.position.z
        );

        transform.position = clampedPosition;
    }

    private bool IsBlockedInDirection(Vector3 direction)
    {
        // Define a distância mínima para a colisão, ajustável dependendo da necessidade
        float moveDistance = direction.magnitude;

        // Se a direção de movimento for muito pequena, não há movimento a ser feito
        if (moveDistance < 0.1f) return false;

        // Lançando o raycast na direção do movimento
        RaycastHit hit;

        // Verifica se o raycast detecta algo na direção do movimento
        if (Physics.Raycast(transform.position, direction, out hit, moveDistance))
        {
            // Verifica se o objeto atingido tem a tag "Colisor"
            if (hit.collider.CompareTag("Colisor"))
            {
                // Se colidir com um objeto com a tag "Colisor", bloqueia o movimento
                return true;
            }
        }

        // Caso contrário, o movimento não está bloqueado
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica se o objeto com o qual o jogador colidiu tem a tag "Colisor"
        if (other.CompareTag("Colisor"))
        {
            // Incrementa o timer com o tempo passado
            damageTimer += Time.deltaTime;

            // Verifica se o tempo passou o intervalo para aplicar o dano
            if (damageTimer >= damageInterval)
            {
                // Reseta o timer
                damageTimer = 0f;

                // Aplica 40 de dano ao jogador
                TakeDamage(10);
            }
        }
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
        if (isPunching)
        {
            Debug.Log("Soco já está em execução.");
            yield break;
        }
        Debug.Log("Iniciando soco.");

        isPunching = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        Debug.Log("Estado da animação: " + stateInfo.shortNameHash);
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


        isPunching = false;
        soco.ActivateProjectile(0);
        soco.ActivateProjectile2(0);

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetInteger("State", 1); // Caminhando
        }
        else
        {
            animator.SetInteger("State", 0); // Idle
        }
    }



    private IEnumerator UpdateRoarState()
    {
        if (isRoaring) yield break; // Evita múltiplas instâncias do mesmo ataque
        isRoaring = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Metade do tempo da animação
        yield return new WaitForSeconds(animationDuration / 2);

        if (!rugido.isRoarActive())
        {
            rugido.RoarAttack(pontoOrigemRugido, transform.rotation);
            rugido.ActivateRoar(1);
        }

        rugido.resetTimer();

        isRoaring = false;

        // Atualiza a animação com base no movimento
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetInteger("State", 1); // Caminhando
        }
        else
        {
            animator.SetInteger("State", 0); // Idle
        }
    }


    private IEnumerator UpdateFartState()
    {
        if (isFarting) yield break; // Evita múltiplas instâncias do mesmo ataque
        isFarting = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Metade do tempo da animação
        yield return new WaitForSeconds(animationDuration / 2);

        peido.FartAttack(transform.position);

        peido.ResetTimer();

        isFarting = false;

        // Atualiza a animação com base no movimento
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetInteger("State", 1); // Caminhando
        }
        else
        {
            animator.SetInteger("State", 0); // Idle
        }
    }


    private IEnumerator UpdateGasAttackState()
    {
        if (isGasAttacking) yield break; // Evita múltiplas instâncias do mesmo ataque
        isGasAttacking = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Metade do tempo da animação
        yield return new WaitForSeconds(animationDuration / 2);

        somCebola.Play();
        gasAttack.PerformGasAttack(transform, transform.rotation);

        gasAttack.ResetTimer();

        isGasAttacking = false;

        // Atualiza a animação com base no movimento
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetInteger("State", 1); // Caminhando
        }
        else
        {
            animator.SetInteger("State", 0); // Idle
        }
    }


    private IEnumerator UpdateBoomerangAttackState()
    {
        if (isBoomerangAttacking) yield break; // Evita múltiplas instâncias do mesmo ataque
        isBoomerangAttacking = true;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Metade do tempo da animação
        yield return new WaitForSeconds(animationDuration / 2);

        boomerangAttack.PerformBoomerangAttack(transform, transform.rotation);

        boomerangAttack.ResetTimer();

        isBoomerangAttacking = false;

        // Atualiza a animação com base no movimento
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetInteger("State", 1); // Caminhando
        }
        else
        {
            animator.SetInteger("State", 0); // Idle
        }
    }


    public void IncreaseAtribute(int at)
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
                speed += 0.4f;
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

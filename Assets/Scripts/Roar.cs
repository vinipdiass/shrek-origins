using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roar : MonoBehaviour
{
    public GameObject roarProjectile;
    private bool isActive;
    private bool Projectile;
    private float timer;
    private float Cooldown;
    private float baseCooldown;
    private int roarCount;
    private float damage;
    private float baseDamage;
    private int evolution;
    public Sprite normalSprite;
    public Sprite specialSprite;

    void Start()
    {
        isActive = false;
        Projectile = false;
        timer = 0f;
        Cooldown = 3f;
        roarCount = 1;
        damage = 55;
        baseDamage = 55;
        evolution = 0;
        baseCooldown = Cooldown;

        SpriteRenderer sr = roarProjectile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = normalSprite;
        }

        Vector3 newScale = roarProjectile.transform.localScale;
        newScale.x = 0.7f;
        newScale.y = 0.7f;
        roarProjectile.transform.localScale = newScale;
    }

    public void addAtributeAttack()
    {
        this.damage += baseDamage;
    }

    public void addAtributeCooldownReduction()
    {
        this.Cooldown -= 0.20f * baseCooldown;
    }

    public void IncreaseTimer()
    {
        timer += Time.deltaTime;
    }

    public bool IsInCooldown()
    {
        return timer < Cooldown;
    }

    public void ActivateRoar(int choice)
    {
        isActive = choice == 1;
    }

    public void ActivateProjectile(int choice)
    {
        Projectile = choice == 1;
    }

    public bool isRoarActive()
    {
        return this.Projectile;
    }

    public void resetTimer()
    {
        this.timer = 0;
    }

    public void evolute()
    {
        if (evolution == 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
        }
        else
        {


            if (evolution == 2)
            {
                SpriteRenderer sr = roarProjectile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = specialSprite;
                    Debug.Log("Sprite do rugido alterado no nível de evolução 2.");
                }
            }
            this.evolution++;
            this.damage += this.baseDamage;

            Debug.Log("Urro evoluído para o nível " + evolution + ".");
        }
    }

    // Método para encontrar o inimigo mais próximo a partir de uma posição
    Transform FindClosestEnemy(Vector3 position)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemyObj in enemies)
        {
            float distance = Vector2.Distance(position, enemyObj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyObj.transform;
            }
        }
        return closestEnemy;
    }

    public void RoarAttack(Transform position, Quaternion rotation)
    {
        // Encontra o inimigo mais próximo a partir da posição de ataque
        Transform targetEnemy = FindClosestEnemy(position.position);

        if (targetEnemy != null)
        {
            // Calcula a direção para o inimigo
            Vector2 directionToEnemy = (targetEnemy.position - position.position).normalized;
            float angleToEnemy = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
            Quaternion baseRotation = Quaternion.Euler(0, 0, angleToEnemy);

            // Projétil principal que busca o inimigo
            GameObject projectile = Instantiate(roarProjectile, position.position, baseRotation);
            Rugido rugidoScript = projectile.GetComponent<Rugido>();
            if (rugidoScript != null)
            {
                rugidoScript.shouldTargetEnemy = true;
                rugidoScript.damage = this.damage;
            }

            // Projéteis adicionais com ângulos levemente diferentes
            if (evolution >= 1)
            {
                // Projétil com +15 graus
                Quaternion rotationOffset = Quaternion.Euler(0, 0, 15);
                Quaternion newRotation = baseRotation * rotationOffset;
                GameObject projectileExtra = Instantiate(roarProjectile, position.position, newRotation);
                Rugido rugidoScriptExtra = projectileExtra.GetComponent<Rugido>();
                if (rugidoScriptExtra != null)
                {
                    rugidoScriptExtra.shouldTargetEnemy = false;
                    rugidoScriptExtra.damage = this.damage;
                }
            }

            if (evolution >= 2)
            {
                // Projétil com -15 graus
                Quaternion rotationOffset = Quaternion.Euler(0, 0, -15);
                Quaternion newRotation = baseRotation * rotationOffset;
                GameObject projectileExtra = Instantiate(roarProjectile, position.position, newRotation);
                Rugido rugidoScriptExtra = projectileExtra.GetComponent<Rugido>();
                if (rugidoScriptExtra != null)
                {
                    rugidoScriptExtra.shouldTargetEnemy = false;
                    rugidoScriptExtra.damage = this.damage;
                }
            }

            if (evolution == 3)
            {
                // Projétil com +30 graus
                Quaternion rotationOffset = Quaternion.Euler(0, 0, 30);
                Quaternion newRotation = baseRotation * rotationOffset;
                GameObject projectileExtra = Instantiate(roarProjectile, position.position, newRotation);
                Rugido rugidoScriptExtra = projectileExtra.GetComponent<Rugido>();
                if (rugidoScriptExtra != null)
                {
                    rugidoScriptExtra.shouldTargetEnemy = false;
                    rugidoScriptExtra.damage = this.damage;
                }
            }
        }
        else
        {
            // Comportamento opcional caso nenhum inimigo seja encontrado
            Debug.Log("Nenhum inimigo encontrado para direcionar o ataque.");
        }

        roarCount++;
    }
}

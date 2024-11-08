using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAttack : MonoBehaviour
{
    public GameObject boomerangProjectilePrefab; // Prefab do projétil
    private bool isActive;
    private float timer;
    private float cooldown;
    private float damage;
    private float baseDamage;
    private int evolution;

    void Start()
    {
        isActive = false;
        timer = 0f;
        cooldown = 3f; // Ajuste conforme necessário
        damage = 80f;
        baseDamage = 80f;
        evolution = 0;
    }

    public void AddAttributeAttack()
    {
        this.damage += baseDamage;
    }

    public void AddAttributeCooldownReduction()
    {
        this.cooldown -= 0.20f * cooldown; // Reduz o cooldown em 20%
    }

    public void IncreaseTimer()
    {
        timer += Time.deltaTime;
    }

    public bool IsInCooldown()
    {
        return timer < cooldown;
    }

    public void ActivateBoomerangAttack(int choice)
    {
        isActive = choice == 1;
    }

    public void ResetTimer()
    {
        this.timer = 0;
    }

    public void Evolute()
    {
        if (evolution >= 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
        }
        else
        {
            evolution++;
            damage += baseDamage;
            Debug.Log("Boomerang Attack evoluído para o nível " + evolution + ".");
        }
    }

    public void PerformBoomerangAttack(Transform position, Quaternion rotation)
    {
        // Encontra um inimigo aleatório
        Transform targetEnemy = FindRandomEnemy();

        if (targetEnemy != null)
        {
            // Instancia o projétil e define o alvo
            GameObject projectile = Instantiate(boomerangProjectilePrefab, position.position, Quaternion.identity);
            BoomerangProjectile boomerangProjectileScript = projectile.GetComponent<BoomerangProjectile>();
            if (boomerangProjectileScript != null)
            {
                boomerangProjectileScript.targetEnemy = targetEnemy;
                boomerangProjectileScript.damage = this.damage;
            }
        }
        else
        {
            Debug.Log("Nenhum inimigo encontrado para direcionar o ataque.");
        }
    }

    // Método para encontrar um inimigo aleatório
    Transform FindRandomEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            int randomIndex = Random.Range(0, enemies.Length);
            return enemies[randomIndex].transform;
        }
        return null;
    }
}

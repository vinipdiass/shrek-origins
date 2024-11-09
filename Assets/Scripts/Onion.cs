using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : MonoBehaviour
{
    public GameObject gasProjectilePrefab; // Prefab do projétil de gás
    private bool isActive;
    private float timer;
    private float cooldown;
    private float damage;
    private float baseDamage;
    private int evolution;
    public Sprite normalSprite;
    public Sprite specialSprite;

    void Start()
    {
        isActive = false;
        timer = 0f;
        cooldown = 3f; // Ajuste o cooldown conforme necessário
        damage = 0f;
        baseDamage = 120f;
        evolution = 0;

        // Inicialize o sprite do projétil, se necessário
        SpriteRenderer sr = gasProjectilePrefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = normalSprite;
        }
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

    public void ActivateGasAttack(int choice)
    {
        isActive = choice == 1;
    }

    public void ResetTimer()
    {
        this.timer = 0;
    }

    public void evolute()
    {
        if (evolution >= 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
        }
        else
        {
            evolution++;
            damage += baseDamage;
            cooldown /= 2;
            Debug.Log("Gas Attack evoluído para o nível " + evolution + ".");
        }
    }

    public void PerformGasAttack(Transform position, Quaternion rotation)
    {
        // Encontra um inimigo aleatório
        Transform targetEnemy = FindRandomEnemy();

        if (targetEnemy != null)
        {
            // Instancia o projétil e define o alvo
            GameObject projectile = Instantiate(gasProjectilePrefab, position.position, Quaternion.identity);
            Cebola gasProjectileScript = projectile.GetComponent<Cebola>();
            if (gasProjectileScript != null)
            {
                gasProjectileScript.targetEnemy = targetEnemy;
                gasProjectileScript.damage = this.damage;
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

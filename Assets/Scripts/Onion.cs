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
    public GameObject gasAreaPrefab;


    void Start()
    {
        isActive = false;
        timer = 0f;
        cooldown = 3f; // Ajuste o cooldown conforme necessário
        damage = 0f;
        baseDamage = 0f;
        evolution = 0;
        GasArea gasArea = gasAreaPrefab.GetComponent<GasArea>();
        gasArea.damagePerSecond = 15f;
        gasArea.areaRadius = 2f;
        // Inicialize o sprite do projétil, se necessário
        SpriteRenderer sr = gasProjectilePrefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = normalSprite;
        }
        
        gasProjectilePrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0f);
        gasAreaPrefab.transform.localScale = new Vector3(8f, 8f, 8f);
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

    public void Evolute()
    {
        if (evolution >= 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
        }
        else
        {
            evolution++;
            
            cooldown /= 2;

            // Aumente o tamanho do projétil de gás
            gasProjectilePrefab.transform.localScale *= 1.2f; // Aumenta em 20% o tamanho a cada evolução
            gasAreaPrefab.transform.localScale *= 1.2f;

            // Aumente o raio da área de gás
            GasArea gasArea = gasAreaPrefab.GetComponent<GasArea>();
            if (gasArea != null)
            {
                gasArea.areaRadius *= 1.2f; // Aumenta o raio em 20% a cada evolução
                gasArea.damagePerSecond += 10;
            }

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

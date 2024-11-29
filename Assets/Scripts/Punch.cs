using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public GameObject socoProjetil;
    private bool isActive;
    private bool Projectile; // Para saber se está realizando o soco
    private bool Projectile2;
    private float timer;
    private float Cooldown;
    private float baseCooldown;
    // Start is called before the first frame update
    private int contaSocos;
    private float damage;
    private float baseDamage;
    private int evolution;
    public Sprite normalSprite;  // Sprite padrão
    void Start()
    {
        isActive = false;
        Projectile = false;
        Projectile2 = false;
        timer = 0f;
        Cooldown = 2f;
        baseCooldown = 2f;
        contaSocos = 1;
        damage = 100;
        evolution = 0;
        baseDamage = damage;

        SpriteRenderer sr = socoProjetil.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = normalSprite;
        }

        Vector3 newScale = socoProjetil.transform.localScale;
        newScale.x = 0.5f;
        newScale.y = 0.5f;
        socoProjetil.transform.localScale = newScale;
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

    public void Activate(int choice)
    {
        if (choice == 0) isActive = false;
        else if (choice == 1) isActive = true;
    }
    public void ActivateProjectile(int choice)
    {
        if (choice == 0)
            Projectile = false;
        else if (choice == 1)
            Projectile = true;
    }
    public void ActivateProjectile2(int choice)
    {
        if (choice == 0)
            Projectile2 = false;
        else if (choice == 1)
            Projectile2 = true;
    }

    public bool isProjectileActive()
    {
        return this.Projectile;
    }
    public bool isProjectile2Active()
    {
        return this.Projectile2;
    }

    public void resetTimer()
    {
        this.timer = 0;
    }

    public void Evolute()
    {
        if (evolution == 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
        }
        else
        {
            this.evolution++;
            this.Cooldown /= this.Cooldown;
            this.damage += this.baseDamage;

            // Dobra as escalas de X e Y do projetil, mantendo o Z
            Vector3 newScale = socoProjetil.transform.localScale;
            newScale.x *= 1.5f;  // Dobra a escala no eixo X
            newScale.y *= 1.5f;  // Dobra a escala no eixo Y
            socoProjetil.transform.localScale = newScale;  // Aplica a nova escala

            Debug.Log("Projetil evoluído. Nova escala: " + socoProjetil.transform.localScale);
        }

    }


    public void attack(Transform position, Quaternion rotation)
    {
        GameObject projectile = Instantiate(socoProjetil, position.position, rotation);
        // Atribui o valor de dano
        Soco socoScript = projectile.GetComponent<Soco>();
        if (socoScript != null)
        {
            socoScript.damage = this.damage;
        }
        contaSocos++;
    }


    // Update is called once per frame
}

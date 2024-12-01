using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public GameObject fartExplosionPrefab; // Prefab for the explosion effect
    private bool isActive;
    private float timer;
    private float cooldown;
    private float damage;
    private float baseDamage;
    private int evolution;
    public float explosionRadius;
    public Sprite normalSprite;
    public Sprite specialSprite;

    void Start()
    {
        isActive = false;
        timer = 0f;
        cooldown = 1.7f; // Adjusted cooldown for balance
        damage = 70f;
        baseDamage = 70f;
        evolution = 0;
        explosionRadius = 2.3f; // Initial radius of the explosion

        // Set initial scale based on explosionRadius
        float newScale = 4f * explosionRadius;
        fartExplosionPrefab.transform.localScale = new Vector3(newScale, newScale, 1f);

        // Optional: Adjust the sprite of the explosion if needed
        if (normalSprite != null)
        {
            SpriteRenderer sr = fartExplosionPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = normalSprite;
            }
        }
    }


    public void AddAttributeAttack()
    {
        this.damage += baseDamage;
    }

    public void IncreaseTimer()
    {
        timer += Time.deltaTime;
    }

    public bool IsInCooldown()
    {
        return timer < cooldown;
    }

    public void ActivateFart(int choice)
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


            if (evolution == 2 && specialSprite != null)
            {
                SpriteRenderer sr = fartExplosionPrefab.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    // Temporarily disable the Animator to set color manually
                    Animator animator = fartExplosionPrefab.GetComponent<Animator>();
                    if (animator != null) animator.enabled = false;

                    sr.color = Color.yellow;
                    Debug.Log("Sprite do peido alterado para amarelo no nível de evolução 2.");

                    // Re-enable the Animator if needed
                    if (animator != null) animator.enabled = true;
                }
            }
            explosionRadius += 0.6f;   // Increase radius
            float newScale = 4f * explosionRadius;
            fartExplosionPrefab.transform.localScale = new Vector3(newScale, newScale, 1f);
            evolution++;
            damage += baseDamage;    // Increase damage with each evolution

            Debug.Log("Peido evoluído para o nível " + evolution + ".");
        }
    }



    public void FartAttack(Vector3 position)
    {
        // Instantiate the explosion effect at the player's position
        GameObject explosion = Instantiate(fartExplosionPrefab, position, Quaternion.identity);

        // Pass the damage and radius values to the explosion instance
        FartExplosion fartExplosionScript = explosion.GetComponent<FartExplosion>();
        if (fartExplosionScript != null)
        {
            fartExplosionScript.damage = this.damage;
            fartExplosionScript.explosionRadius = this.explosionRadius;
        }

        ResetTimer();
    }

    // Optional: Visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

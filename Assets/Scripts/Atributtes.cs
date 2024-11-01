using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atributtes : MonoBehaviour
{

    // Níveis
    public int levelMaxLife = 0;
    public int levelRecovery = 0;
    public int levelDamage = 0;
    public int levelCooldown = 0;

    // Atualiza o dano e incrementa o nível de dano

    public int getLevelMaxLife()
    {
        return levelMaxLife;
    }

    public int getLevelRecovery()
    {
        return levelRecovery;
    }

    public int getLevelDamage()
    {
        return levelDamage;
    }

    public int getLevelCooldown()
    {
        return levelCooldown;
    }

    public void increaseLevelDamage()
    {
        levelDamage++;
    }

    public void increaseLevelMaxLife()
    {
        levelMaxLife++;
    }

    public void increaseLevelRecovery()
    {
        levelMaxLife++;
    }

    public void increaseLevelCooldown(){
        levelCooldown++;
    }
}
using System;
using System.Collections.Generic;


[System.Serializable]
public class PlayerData
{
    public List<bool> mapasDesbloqueados;
    public int dinheiro;
    public List<int> atributosDisponiveis;

    public PlayerData()
    {
        mapasDesbloqueados = new List<bool>();
        dinheiro = 0;
        atributosDisponiveis = new List<int>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstagioModel
{
    public int Elemento1;
    public int Elemento2;
    public int Elemento3;

    public EstagioModel()
    {
        // 0 = moeda / 1 = nada / 2 = bloco

        Elemento1 = Random.Range(0, 3);
        Elemento2 = Random.Range(0, 3);
        Elemento3 = Random.Range(0, 3);

        checarRegras();
    }

    private void checarRegras()
    {
        // Se existe mais de uma moeda
        if(Elemento1 == 0 && Elemento2 == 0)
        {
            Elemento1 = Random.Range(0, 3);
            Elemento2 = Random.Range(0, 3);

            checarRegras();
        }

        // Se existe mais de uma moeda
        if(Elemento1 == 0 && Elemento3 == 0)
        {
            Elemento1 = Random.Range(0, 3);
            Elemento3 = Random.Range(0, 3);

            checarRegras();
        }

        // Se existe mais de uma moeda
        if(Elemento2 == 0 && Elemento3 == 0)
        {
            Elemento2 = Random.Range(0, 3);
            Elemento3 = Random.Range(0, 3);

            checarRegras();
        }

        // Se todos for bloco abre um caminho
        if (Elemento1 == 2 && Elemento2 == 2 && Elemento3 == 2) 
        {
            Elemento2 = Random.Range(0, 2);
        }
    }
}

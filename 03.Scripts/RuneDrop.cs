using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrop : HpCtrl
{ 

    public GameObject[] Runes;

    private void Droprune()
    {
        GameObject selectRune = Runes[Random.Range(0, Runes.Length)];
        Instantiate(selectRune, transform.position, Quaternion.identity);

    }

}
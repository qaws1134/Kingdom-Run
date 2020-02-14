using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itween_gold : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        int mi= -1;
        int ran = Random.Range(0, 2);
        if(ran ==0)
        {
            mi *= mi;
        }
        iTween.MoveBy(obj, iTween.Hash("x", 0.5*mi,"y",1, "easeQutExpo", iTween.EaseType.easeOutQuart));

    }
}

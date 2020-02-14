using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itween_Rune2 : MonoBehaviour
{

    public GameObject obj;

    void Start()
    {
        var dr = GameObject.FindGameObjectWithTag("RUNE");
        
        Instantiate(obj);
        iTween.MoveBy(obj, iTween.Hash("x", 190, "y", 280, "easeType", iTween.EaseType.easeInQuart));
        Destroy(this.gameObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//파워업
public class RedRune : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rm = GameObject.Find("Player").GetComponent<RuneManager>();
        if (other.CompareTag("PLAYER"))
        {
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            iTween.MoveTo(this.gameObject, iTween.Hash("x", 2.2, "y", 3.5, "time", 0.8, "easeType", iTween.EaseType.easeInQuart));
            rm.Red = true;
            Destroy(this.gameObject, 0.8f);
        }
        else if (other.CompareTag("DESTROY"))
        {
            rm.Ground_RuneNum--;
            Destroy(this.gameObject);
        }
    }
}

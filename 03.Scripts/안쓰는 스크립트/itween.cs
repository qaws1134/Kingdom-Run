using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itween : MonoBehaviour
{
    public GameObject obj;
    float repeatTime = 5.0f;
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {

        do
        {
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float wSize = Camera.main.orthographicSize * screenRatio;
            float offset = 2.5f;

            int temp = Random.Range(0, 2);
            if (temp == 0)
                temp = -1;

            float moveItween = 2 * temp;
            if (obj.transform.position.x + moveItween >= wSize - offset)
            {
                moveItween *= -1;
            }
            else if (obj.transform.position.x + moveItween <= -wSize + offset)
            {
                moveItween *= -1;
            }

            iTween.MoveBy(obj, iTween.Hash("x", moveItween, "time", 2.0f));

            repeatTime = Random.Range(4.0f, 7.0f);     // 움직임 패턴 반복 주기를 랜덤으로 설정
            yield return new WaitForSeconds(repeatTime);
        } while (true);
    }
}


/*      좌우로 벗어나지 못하게
    float screenRatio = (float)Screen.width / (float)Screen.height;
        float wSize = Camera.main.orthographicSize * screenRatio;
        if (obj.position.x >= wSize - offset)
        {
            obj.position = new Vector3(wSize - offset, obj.position.y, 0);
        }
        if (obj.position.x <= -wSize + offset)
        {
            obj.position = new Vector3(-wSize + offset, obj.position.y, 0);
        }

    */

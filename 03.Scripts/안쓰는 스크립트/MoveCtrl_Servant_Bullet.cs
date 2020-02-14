using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//소환수의 총알을 
public class MoveCtrl_Servant_Bullet : MonoBehaviour
{
    public Transform tr;
    public float speed = 0;

    void Update()
    {
        tr.Translate(Vector3.up * speed * Time.deltaTime); // Vector3.up 방향으로 speed값의 속도로 총알의 위치를 바꿔줌   
    }
}

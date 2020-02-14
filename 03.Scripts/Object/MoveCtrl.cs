using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 오브젝트의 이동을 구현하는 스크립트
public class MoveCtrl : MonoBehaviour
{
    public Transform tr;
    public float speed = 0;

    void Update()
    {
        tr.Translate(Vector3.up * speed * Time.deltaTime, Space.World); // Vector3.up 방향으로 speed값의 속도로 총알의 위치를 바꿔줌     
    }
}
    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 삭제 ㄴㄴ 나중에 쓸데 있을듯.

public class RotatePoint : MonoBehaviour
{
    private float RotateSpeed = 4f; // 회전하는 속도
    private float Radius = 0.9f;    // 반지름
    public GameObject player;
    private Vector2 _centre;
    private float _angle;

    private void Start()
    {
        player = GameObject.Find("Player");     // 회전 하고자 하는 중심 오브젝트. 지정
    }

    private void Update()
    {
        _centre = player.transform.position;
        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle) + 1.05f) * Radius;
        this.transform.position = _centre + offset;
    }
}

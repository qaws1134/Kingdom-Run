using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public JoyStick joystick;   // 조이스틱 스크립트
    public float MoveSpeed; // 플레이어 이동속도

    private Vector3 _moveVector;    //플레이어 이동 벡터
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform; // 트랜스폼 캐싱
        _moveVector = Vector3.zero; // 플레이어 이동벡터 초기화
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();    // 터치패드 입력받기
    }
    private void FixedUpdate()
    {
        // 플레이어 이동
        Move();
    }
    public void HandleInput()
    {
        _moveVector = poolInput();
    }
    public Vector3 poolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
        Vector3 moveDir = new Vector3(h, v, 0).normalized;

        return moveDir;
    }

    public void Move()
    {
        _transform.Translate(_moveVector * MoveSpeed * Time.deltaTime);
    }
}

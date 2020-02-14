using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//die 프리팹이 나온 뒤 일정시간 후에 없애기 위한 스크립트
public class DestoryObj : MonoBehaviour
{
    public float DestroySec = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, DestroySec);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class JoyStick : MonoBehaviour
{
    private Image bgImg;    // 조이스틱 배경
    private Image joystickImg; // 조이스틱
    private Vector3 inputVector; // 이동 벡터값

    // Start is called before the first frame update
    void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Debug.Log("JoyStick >>> OnDrage()");

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, pos.y * 2, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            // 조이스틱 이동
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped) // 터치하고 있을 때
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)   // 터치 안 할때 원위치
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
    public float GetHorizontalValue()   // 플레이어 컨트롤 스크립트에서 X값을 받기 위함
    {
        return inputVector.x;
    }

    public float GetVerticalValue() // 플레이어 컨트롤 스크립트에서 Y값을 받기 위함
    {
        return inputVector.y;
    }
}

using PedometerU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealRun : MonoBehaviour
{
    // 걸음
    public Text stepText, distanceText;
    public Pedometer pedometer;

    // 신발
    public Text get_shoes_count_text;
    public Text get_gold_count_text;
    public int get_shoes_count;
    public int get_gold_count;

    // Start is called before the first frame update
    public void Start()
    {
        pedometer = new Pedometer(OnStep);
        OnStep(0, 0);
        print_shoes_count();
        print_gold_count();
    }

    public void OnStep(int steps, double distance)
    {
        // Display the values // Distance in feet
        stepText.text = steps.ToString();
        distanceText.text = (distance).ToString("F2");      //F2는 소수점 아래 두 개까지 라는 뜻

        // 골드
        get_gold_count = steps;
        print_gold_count();
        
        //슈즈
        if (steps % 100 == 0 && steps > 0)    // 임시 테스트로 10 걸음
        {
            get_shoes_count += 1;
            print_shoes_count();
        }
    }

    public void print_shoes_count()
    {
        get_shoes_count_text.text = get_shoes_count.ToString();
    }

    public void print_gold_count()
    {
        get_gold_count_text.text = get_gold_count.ToString();
    }

    public void OnDisable()
    {
        // Release the pedometer
        pedometer.Dispose();
        pedometer = null;
    }
}

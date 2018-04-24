using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCounter : MonoBehaviour {

   
    public Text dayText;

    float timeAmt;
    float time;
    float dayTime = 10;

    int days;


    // Use this for initialization
    void Start()
    {
        days = 0;
        timeAmt = dayTime;
        time = timeAmt;
    }

    // Update is called once per frame
    void Update()
    {

        // make function to check for npc attack to deal damage to player 

        if (time > 0)
        {
            time -= Time.deltaTime;
            
        }

        if (time <= 0)
        {
            // add day +1
            days++;
            dayText.text = days.ToString();
            timeAmt = dayTime;
            time = timeAmt;
        }

        dayText.text = days.ToString();
    }


}

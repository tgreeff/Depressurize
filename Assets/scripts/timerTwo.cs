using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerTwo : MonoBehaviour {

    Image fillImg;
    float timeAmt = 300;
    float time;

    private int dmg = 0;

	// Use this for initialization
	void Start () {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
	}
	
	// Update is called once per frame
	void Update () {

        // make function to check for npc attack to deal damage to player 

        if (time > 1)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt; // 9/10, 8/10, 7/10 ....
        }
	}

    void dealDamage(int damage)
    {
        // do some damage to player here

        dmg = damage;
    }
}

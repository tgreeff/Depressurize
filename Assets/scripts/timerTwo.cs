using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerTwo : MonoBehaviour {

    Image fillImg;
    float timeAmt = 10;
    float time; 

	// Use this for initialization
	void Start () {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
	}
	
	// Update is called once per frame
	void Update () {
        if (time > 1)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt; // 9/10, 8/10, 7/10 ....
        }
	}
}

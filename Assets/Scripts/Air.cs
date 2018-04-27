using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Air : MonoBehaviour {

    Image fillImg;
    float timeAmt = 300;
    public float time;


    // Use this for initialization
    void Start()
    {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
    }

    // Update is called once per frame
    void Update()
    {

        // make function to check for npc attack to deal damage to player 

        if (time < 0.0)
        {
            toDeath();
        }

        if (time > 1)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt; // 9/10, 8/10, 7/10 ....
        }

        fillImg.fillAmount = time / timeAmt;
    }

    void toDeath()
    {
        SceneManager.LoadScene("Die");
    }

}

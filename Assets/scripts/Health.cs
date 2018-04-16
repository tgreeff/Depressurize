using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public Image fillImg;
    float remainingAmt = 300;
    private float dmg = 0;


    private void Start()
    {
        dmg = remainingAmt;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dmg < 0.0)
        {
            toDeath();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Shootable")
        {
            dmg -= 20;
            fillImg.fillAmount = dmg / remainingAmt;
        }
    }

    void toDeath()
    {
        SceneManager.LoadScene("Die");
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour {

    public GameObject myBox;
    public GameObject myGun;




    // Use this for initialization
    void Start () {
		myBox.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("2"))
        {
            myBox.SetActive(false);
            GetComponent<PlayerIO>().enabled = false;
        }

        if (Input.GetKeyDown("1"))
        {
            
            myBox.SetActive(true);
            GetComponent<PlayerIO>().enabled = true;
        }

    

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            other.gameObject.SetActive(false);
            GetComponent<PlayerIO>().numBlocks += 10;
            myGun.GetComponent<RayCastShootComplete>().ammo += 10;
            
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            GetComponent<PlayerIO>().numBlocks += 10;
        }

    }


}

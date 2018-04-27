using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour {

    public GameObject myBox;
    public GameObject myGun;




    // Use this for initialization
    void Start () {
      
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
            myGun.GetComponent<RayCastShootComplete>().ammo += 10;
            
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            GetComponent<PlayerIO>().numBlocks += 10;
        }

        if (other.gameObject.CompareTag("Water"))
        {
            other.gameObject.SetActive(false);
            GetComponent<Water>().time += 20;
        }

        if (other.gameObject.CompareTag("Air"))
        {
            other.gameObject.SetActive(false);
            GetComponent<Air>().time += 20;
        }

        if (other.gameObject.CompareTag("Health"))
        {
            other.gameObject.SetActive(false);
            GetComponent<Health>().dmg += 20;
        }

        if (other.gameObject.CompareTag("Ash"))
        {
            other.gameObject.SetActive(false);
        }

    }


}

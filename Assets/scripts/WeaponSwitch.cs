using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour {

    public GameObject myBox;
    public GameObject myTurret;
	public GameObject myGun;

	public GameObject buildImg;
	public GameObject handImg;

	public GameObject oxygen;
	public GameObject water;

	// Use this for initialization
	void Start () {
		buildImg.SetActive(false);
		handImg.SetActive(true);
		myBox.SetActive(false);
		myTurret.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			buildImg.SetActive(true);
			handImg.SetActive(false);
			myTurret.SetActive(false);
			myBox.SetActive(true);
			GetComponent<PlayerIO>().enabled = true;
		}

		if (Input.GetKeyDown("2")) {
			buildImg.SetActive(true);
			handImg.SetActive(false);
			myTurret.SetActive(true);
			myBox.SetActive(false);
			GetComponent<PlayerIO>().enabled = true;
		}
		if (Input.GetKeyDown("3"))
        {
			buildImg.SetActive(false);
			handImg.SetActive(true);
			myTurret.SetActive(false);
			myBox.SetActive(false);
            GetComponent<PlayerIO>().enabled = false;
        }
    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ammo")) {
			Destroy(other.gameObject);
			GetComponent<PlayerIO>().numBlocks += 10;
            myGun.GetComponent<RayCastShootComplete>().ammo += 10;        
        }

        if (other.gameObject.CompareTag("Coin")){
			Destroy(other.gameObject);
            GetComponent<PlayerIO>().numBlocks += 10;
        }

		if (other.gameObject.CompareTag("Oxygen")) {
			Destroy(other.gameObject);
			oxygen.GetComponent<Oxygen>().dmg += 100;

		}
		if (other.gameObject.CompareTag("Water")) {
			Destroy(other.gameObject);
			water.GetComponent<WaterScript>().dmg += 100;
		}
		if (other.gameObject.CompareTag("Health")) {
			Destroy(other.gameObject);
			GetComponent<Health>().dmg += 50;

		}
		if (other.gameObject.CompareTag("Ash")) {
			Destroy(other.gameObject);
		}
	}


}

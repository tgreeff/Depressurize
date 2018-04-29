using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour {

    public GameObject myBox;
    public GameObject myTurret;
	public GameObject myGun;

	public Image buildImg;
	public Image handImg;

	// Use this for initialization
	void Start () {
		myBox.SetActive(false);
		myTurret.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			myTurret.SetActive(false);
			myBox.SetActive(true);
			GetComponent<PlayerIO>().enabled = true;
		}

		if (Input.GetKeyDown("2")) {
			myTurret.SetActive(true);
			myBox.SetActive(false);
			GetComponent<PlayerIO>().enabled = true;
		}
		if (Input.GetKeyDown("3"))
        {
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
			GetComponent<PlayerIO>().numBlocks += 10;

		}
		if (other.gameObject.CompareTag("Water")) {
			Destroy(other.gameObject);

		}
		if (other.gameObject.CompareTag("Health")) {
			Destroy(other.gameObject);
			GetComponent<PlayerIO>().numBlocks += 10;

		}
		if (other.gameObject.CompareTag("Ash")) {
			Destroy(other.gameObject);
		}
	}


}

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
            myGun.SetActive(false);
            myBox.SetActive(true);
            GetComponent<PlayerIO>().enabled = true;
        }

        if (Input.GetKeyDown("3"))
        {
            myGun.SetActive(true);
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathText : MonoBehaviour {

    int deathPref;
    public Text dayText;

    // Use this for initialization
    void Start () {
        deathPref = PlayerPrefs.GetInt("days");
        toScreen();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void toScreen()
    {
        dayText.text = ("You Died and only survived " + deathPref.ToString() + " days better luck next time.");
    }

}

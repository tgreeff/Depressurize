using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaterScript : MonoBehaviour {

	public Image fillImg;
	float remainingAmt = 300;
	public float dmg = 0;
	private float scale;


	private void Start() {
		dmg = remainingAmt;
		scale = 0.5f;
	}

	// Update is called once per frame
	void Update() {
		fillImg.fillAmount = dmg / remainingAmt;
		if (dmg < 0.0) {
			toDeath();
		}
		if(dmg > 0) {
			dmg -= scale * Time.deltaTime;
		}
	}

	void toDeath() {
		SceneManager.LoadScene("Die");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Oxygen : MonoBehaviour {

	public Image fillImg;
	float remainingAmt = 300;
	public float dmg = 0;
	private float scale;


	private void Start() {
		dmg = remainingAmt;
		scale = 3f;
	}

	// Update is called once per frame
	void Update() {
		dmg -= scale * Time.deltaTime;
		fillImg.fillAmount = dmg / remainingAmt;
		if (dmg < 0.0) {
			toDeath();
		}

	}

	void toDeath() {
		SceneManager.LoadScene("Die");
	}
}

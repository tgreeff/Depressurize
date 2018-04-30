using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class compassScript : MonoBehaviour {

    public RawImage compass;
    public Transform player;

	
	// Update is called once per frame
	void Update () {
        compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
	}
}

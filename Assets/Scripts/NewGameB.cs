using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameB : MonoBehaviour {

    public Button btn;

	// Use this for initialization
	void Start () {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(LoadByIndex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadByIndex()
    {
        SceneManager.LoadScene("GameWorld");
    }
}

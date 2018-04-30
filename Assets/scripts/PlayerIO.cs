using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerIO : MonoBehaviour {

    RaycastHit hit;
    int maxBuild;
	public Transform box;
	public Transform turret;
	public Transform retAdd;
    public Transform retDelete;
	public GameObject retTurretAdd;
	public GameObject retTurretDelete;
	public int numBlocks;

    private bool isTrue = false;

    // Use this for initialization
    void Start () {
		if(retAdd == null) {
			retAdd = GameObject.Find("RetAdd").transform;
		}
        
		if(retDelete == null) {
			retDelete = GameObject.Find("RetDelete").transform;
		}

		if (retTurretAdd == null) {
			retTurretAdd = GameObject.Find("RetTurretAdd");
		}

		if (retTurretDelete == null) {
			retTurretDelete = GameObject.Find("RetTurretDelete");
		}

	}

	// Update is called once per frame
	void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2), 0)), out hit, Mathf.Infinity))
        {
			bool turretActive = gameObject.GetComponent<WeaponSwitch>().myTurret.activeSelf;
			bool boxActive = gameObject.GetComponent<WeaponSwitch>().myBox.activeSelf;
			retAdd.GetComponent<Renderer>().enabled = true;            
            retAdd.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
			retTurretAdd.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);

			if (hit.transform.tag == "Block")
            {
                retDelete.transform.position = hit.transform.position;
                retDelete.GetComponent<Renderer>().enabled = true;
				retTurretDelete.SetActive(false);

			} else {
				retDelete.GetComponent<Renderer>().enabled = false;
			}

			if(hit.transform.tag == "Turret") {
				retTurretDelete.transform.position = hit.transform.position;
				retTurretDelete.SetActive(true);
			} else {
				retTurretDelete.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.E) && numBlocks > 0 && turretActive)
            {
				Vector3 pos = retTurretAdd.transform.position;
				Vector3 newPos = new Vector3(pos.x, pos.y - 1.2f, pos.z);
                numBlocks--;
                Instantiate(turret, newPos, Quaternion.identity);
				retTurretDelete.SetActive(true);
            }
			else if (Input.GetKeyDown(KeyCode.E) && numBlocks > 0 && boxActive)
			{
				Vector3 pos = retTurretAdd.transform.position;
				Vector3 newPos = new Vector3(pos.x, pos.y - 0.5f, pos.z);
				numBlocks--;
				Instantiate(box, newPos, Quaternion.identity);
				retDelete.GetComponent<Renderer>().enabled = false;
				retTurretDelete.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Q))
            {
				string tag = hit.transform.gameObject.tag;
				if (tag == "Block" || tag == "Turret") {
					Destroy(hit.transform.gameObject);
					retTurretDelete.SetActive(false);
				}
			}

        }
        else
        {
            retAdd.GetComponent<Renderer>().enabled = false;
            retDelete.GetComponent<Renderer>().enabled = false;
			retTurretAdd.SetActive(false);
			retTurretDelete.SetActive(false);
		}
    }
}

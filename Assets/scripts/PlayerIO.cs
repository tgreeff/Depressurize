using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIO : MonoBehaviour {

    RaycastHit hit;
    int maxBuild;
    Transform retAdd;
    Transform retDelete;

    private bool isTrue = false;

    // Use this for initialization
    void Start () {
        retAdd = GameObject.Find("RetAdd").transform;
        retDelete = GameObject.Find("RetDelete").transform;
    }

    // Update is called once per frame
    void Update()
    {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2), 0)), out hit, Mathf.Infinity))
            {
                retAdd.GetComponent<Renderer>().enabled = true;
                
                retAdd.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);

                if (hit.transform.tag == "Block")
                {
                    retDelete.transform.position = hit.transform.position;
                    retDelete.GetComponent<Renderer>().enabled = true;

            }
                if (hit.transform.tag != "Block")
                {
                    retDelete.GetComponent<Renderer>().enabled = false;
                }
                if (Input.GetKeyDown("5"))//GetMouseButtonDown(1))
                {
                    GameObject block = (GameObject)Instantiate(Resources.Load("Box"), retAdd.transform.position, Quaternion.identity);
                }

                else if (Input.GetKeyDown("0"))
                {
                Destroy(hit.transform.gameObject);
                }
            }
        else
        {
            retAdd.GetComponent<Renderer>().enabled = false;
            retDelete.GetComponent<Renderer>().enabled = false;
        }
    }

    
    void canBuild()
    {
        if (Input.GetKeyDown("5"))
        {
            isTrue = !isTrue;
        }
    }




}

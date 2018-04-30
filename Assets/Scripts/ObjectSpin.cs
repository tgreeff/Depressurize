using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour {

    public float speed = 15f;


    void Update()
    {
        //  transform.Rotate(Vector3.up, speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, 5, 0), Space.Self);
    }

    
	
	
}

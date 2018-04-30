using UnityEngine;
using System.Collections;

public class SimpleShoot : MonoBehaviour
{

    public int ammo = 99;
    public GameObject bulletPF;
    public GameObject sp;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0)
        {
            ammo--;
            Instantiate(bulletPF, sp.transform.position, sp.transform.rotation);
            sp.GetComponent<AudioSource>().Play();
        }
    }

}

using UnityEngine;
using System.Collections;

using UnityEngine.UI; // IMPORTANT TO HAVE THIS.

public class HoloAmmoText : MonoBehaviour
{

    public GameObject rifle;

    Text ammoText;

    void Awake()
    {
        ammoText = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = rifle.GetComponent<SimpleShoot>().ammo.ToString();
    }

}

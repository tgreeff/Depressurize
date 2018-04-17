using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuitAmmo : MonoBehaviour
{

    public GameObject handCannon;
    

    Text textAmmo;
    //Text textBuild;
    //public Text shown;



    private void Awake()
    {
        textAmmo = GetComponent<Text>();
     //   textBuild = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
     //   shown.text = textAmmo.text;
    }

    // Update is called once per frame
    void Update()
    {

       
        textAmmo.text = handCannon.GetComponent<RayCastShootComplete>().ammo.ToString();
      //  textBuild.text = handCannon.GetComponent<PlayerIO>().numBlocks.ToString();
    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildAmmo : MonoBehaviour {

    public GameObject myBlock;


    Text block;
    //Text textBuild;
    //public Text shown;



    private void Awake()
    {
        block = GetComponent<Text>();
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


        block.text = myBlock.GetComponent<PlayerIO>().numBlocks.ToString();
        //  textBuild.text = handCannon.GetComponent<PlayerIO>().numBlocks.ToString();
    }




}

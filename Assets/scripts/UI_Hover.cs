using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hover : MonoBehaviour
{

    public GameObject myPanel;
   

    void Start()
    {
      
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))
        {
            myPanel.SetActive(true);
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            myPanel.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        myPanel.SetActive(true);
    }

    void OnMouseExit()
    {
        myPanel.SetActive(false);
    }
}
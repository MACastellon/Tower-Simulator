using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomToggle : MonoBehaviour
{
    Toggle _toggle;
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    public void ZoomToggleBool(bool state)
    {
        bool theState = state;
        if (theState)
        {
            CameraDrag.instance.ZoomOut();
        }
        else
        {
            CameraDrag.instance.ZoomIn();
        }
       
    }


   
}

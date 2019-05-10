using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    OutputMenus outputMenus;

    public bool startPlay = false;
    // Start is called before the first frame update
    void Start()
    {
        outputMenus = GetComponent<OutputMenus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlay)
        {
            outputMenus.isfade = true;

            if (Input.anyKey && outputMenus.isfade)
            {
                outputMenus.isStart = true;
            }
        }
    }


}

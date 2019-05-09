using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCommand : MonoBehaviour
{
    [SerializeField]
    Color pressText;

    [SerializeField]
    private float fadespeed = 0.1f;

    bool tofade = false;

    // Start is called before the first frame update
    void Start()
    {
        pressText = this.GetComponent<Text>().color;
        pressText.a = 1;
        GetComponent<Text>().color = pressText;
    }

    // Update is called once per frame
    void Update()
    {
        if(tofade)
        {
            pressText.a += fadespeed;
            GetComponent<Text>().color = pressText;
            if(pressText.a >= 1)
            {
                tofade = false;
            }
        }


        if (!tofade)
        {
            pressText.a -= fadespeed;
            GetComponent<Text>().color = pressText;
            if (pressText.a <= 0)
            {
                tofade = true;
            }
        }
    }
}

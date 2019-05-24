using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputMenus : MonoBehaviour
{
    [SerializeField]
    Text start;
    
    [SerializeField]
    Image Play;

    [SerializeField]
    Image Exit;

    [SerializeField]
    Image Cursor;

    CursorController cursorController;

    public bool isfade = false;
    public bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        start.enabled = false;
        Play.enabled = false;
        Exit.enabled = false;
        cursorController = Cursor.GetComponent<CursorController>();
        Cursor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isfade && !isStart)
        {
            start.enabled = true;
        }

        if (isStart)
        {            
            Play.enabled = true; 
            Exit.enabled = true;
            Cursor.enabled = true;
            cursorController.isActive = true;
            start.enabled = false;
        }

    }
}

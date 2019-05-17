using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputMenus : MonoBehaviour
{
    [SerializeField]
    Text start;

    [SerializeField]
    Text Message;

    [SerializeField]
    Text Play;

    [SerializeField]
    Text Option;

    [SerializeField]
    Text Exit;

    [SerializeField]
    Image Cursor;

    CursorController cursorController;

    public bool isfade = false;
    public bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        start.enabled = false;
        Message.enabled = false;
        Play.enabled = false;
        Option.enabled = false;
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
            Message.enabled = true;
            Play.enabled = true;
            Option.enabled = true;
            Exit.enabled = true;
            Cursor.enabled = true;
            cursorController.isActive = true;
            start.enabled = false;
        }

    }
}

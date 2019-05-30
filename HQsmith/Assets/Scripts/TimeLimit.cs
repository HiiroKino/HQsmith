using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    [SerializeField]
    private int minute;

    [SerializeField]
    private float seconds;
    //前のupdate時の秒数
    private float oldSeconds;
    //たいまー表示用テキスト
    private Text timerText;

    /*[SerializeField]
    Text LimitCount;*/

    [SerializeField]
    Text timeUp;

    public bool isFinished = false;

    // Use this for initialization
    void Start()
    {

        oldSeconds = 0;

        //LimitCount.enabled = false;

        timeUp.enabled = false;

        timerText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        seconds -= Time.deltaTime;

        if (seconds <= 0)
        {
            minute--;
            seconds = seconds + 60;

        }

        //値が変わった時だけテキストUIを更新
        if ((int)seconds != (int)oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
        }
        oldSeconds = seconds;

        if(minute <= 0 && (int)seconds <= 0)
        {
            timerText.enabled = false;

            timeUp.enabled = true;
        }

        /*//残り5秒になったら残り時間を2秒表示
        if (minute <= 0 && (int)seconds <= 10)
        {
            LimitCount.enabled = true;

            if ((int)seconds <= 8)
            {
                LimitCount.enabled = false;
            }
        }*/
    }
}
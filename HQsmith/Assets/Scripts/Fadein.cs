using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    [SerializeField]
    float speed = 0.05f;

    private Color ImageColor;

    TitleController titleController;

    float alfa;
    float red, green, blue;



    // Start is called before the first frame update
    void Start()
    {
        titleController = GameObject.Find("TitleController").GetComponent<TitleController>();

        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;


        ImageColor = this.GetComponent<Image>().color;
        ImageColor = new Color(red, green, blue, alfa);
    }

    // Update is called once per frame
    void Update()
    {
        if (ImageColor.a <= 1)
        {

            ImageColor.a += speed;　//アルファ値を徐々に＋する

            this.GetComponent<Image>().color = ImageColor;　//テキストへ反映させる

        }

        if (Input.anyKey)
        {
            ImageColor.a = 1;
            this.GetComponent<Image>().color = ImageColor;
            titleController.startPlay = true;
        }

        if(ImageColor.a >= 1 && !titleController.startPlay)
        {
            titleController.startPlay = true;
        }
    }
}

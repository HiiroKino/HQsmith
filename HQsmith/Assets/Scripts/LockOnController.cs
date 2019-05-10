﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnController : MonoBehaviour
{
    List<GameObject> list = new List<GameObject>();

    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.position = new Vector3(transform.position.x,
                                                0,
                                                transform.position.z);  
    }

    //ロックオンボタンを押されたときにロックオンをする相手を決めるためのメソッド
    public GameObject LockOnTarget(int target)
    {
        GameObject MainTarget;

        for (int i = 1; i < list.Count; i++)
        {
            float x = AngleSort(list[i]);
            float y = AngleSort(list[i-1]);

            for (int j = i; 0 < j ; j--)
            {
                if(x < y)
                {
                    //ソートの保存用の変数群
                    GameObject Storage1 = list[i];
                    list[i] = list[i - 1];
                    list[i - 1] = Storage1;
                }
            } 
        }

        MainTarget = list[0]; 

        for(int x = 1; x < list.Count; x++)
        {
            if(System.Math.Abs(AngleSort(list[x-1])) > System.Math.Abs(AngleSort(list[x])))
            {
                if (list.Count - 1 < x + target)
                {
                    MainTarget = list[0];
                }
                else if (0 > x + target)
                {
                    MainTarget = list[list.Count - 1];
                }
                else
                {
                    MainTarget = list[x + target];
                }
            }
        } 
        return MainTarget;
    }

    //ソート用の値を出すためのメソッド
    float AngleSort(GameObject obj)
    {
        Vector3 diff = obj.transform.position - cameraTransform.position;

        Vector3 axis = Vector3.Cross(cameraTransform.forward, diff);

        float angle = Vector3.Angle(cameraTransform.forward, diff)
                                * (axis.y < 0 ? -1 : 1);

        return angle;
    }

    //カメラ内に入ったオブジェクトをListにAddするためのメソッド
    public void TargetListAdd(GameObject obj)
    {
        list.Add(obj);
    }

    //カメラ内から出たオブジェクトをListからRemoveするためのメソッド
    public void TargetListRemove(GameObject obj)
    {
        list.Remove(obj);
    }   
}

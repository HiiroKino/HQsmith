using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ユーザーコントローラーから送られてきた値をもらうための関数
    float m_horizontal;
    float m_vertical;

    //ガード用のフラグ
    bool m_guardFlag;
    
    float MoveSpeed = 3f;
    float StepIntervalTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //移動処理用関数
        MovePlayer();
        
        //ステップインターバルタイマー
        if(StepIntervalTimer > 0)
        {
            StepIntervalTimer -= Time.deltaTime; 
        }
    }

    //移動処理
    public void MovePlayer()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * m_vertical + Camera.main.transform.right * m_horizontal;
        Debug.Log(cameraForward + " " + moveForward);

        if (m_guardFlag == false)
        {
            //通常移動処理
            // 移動方向に進ませる
            transform.position += moveForward * MoveSpeed * Time.deltaTime;

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                //                transform.rotation = Quaternion.LookRotation(moveForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveForward), 0.3f);
            }
        }
        else if(StepIntervalTimer <= 0 && m_guardFlag )
        {
            //ステップ処理
            if (Mathf.Abs(moveForward.x) >= 0.5 || Mathf.Abs(moveForward.z) >= 0.5)
            {
                Debug.Log("ステップ");
                //ステップのインターバル
                StepIntervalTimer = 0.7f;
                //移動方向にステップ
                transform.position += moveForward * 30f * Time.deltaTime;
            }
        }
    }

    //ガード処理
    public void GuardFlag(bool value)
    {
        m_guardFlag = value;
    }

    //ダッシュ処理
    public void DashFlag(bool value)
    {
        if(value == true)
        {
            MoveSpeed = 20f;
        }
        else
        {
            MoveSpeed = 3f;
        }
    }

    public void PlayAnimation(string str)
    {
        Debug.Log(str);
    }

    public float Horizontal
    {
        set
        {
            m_horizontal = value;
        }
    }

    public float Vertical
    {
        set
        {
            m_vertical = value;
        }
    }
}

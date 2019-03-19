using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ユーザーコントローラーから送られてきた値をもらうための関数
    float m_horizontal;
    float m_vertical;

    Rigidbody m_rigidbody;

    //ガード用のフラグ
    bool m_guardFlag;
    
    float MoveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動処理用関数
        MovePlayer();
    }

    //移動処理
    public void MovePlayer()
    {
        if (m_guardFlag == false)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * m_vertical + Camera.main.transform.right * m_horizontal;

            // 移動方向に進ませる
            m_rigidbody.velocity = moveForward * MoveSpeed + new Vector3(0, m_rigidbody.velocity.y, 0);

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else
        {
            Debug.Log("ガード中ですよー");
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

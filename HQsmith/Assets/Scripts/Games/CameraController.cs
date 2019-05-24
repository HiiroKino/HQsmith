using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject target; // an object to follow
    public Vector3 offset; // offset form the target object

    private GameObject LockOnTarget = null;

    LockOnController m_lockOnController;
    [SerializeField]
    PlayerController m_playerController;
    
    [SerializeField]
    float m_lockOnYPosition;
    [SerializeField]
    float cameraDistance;

    public KeyCode m_lockOnKey = KeyCode.Joystick1Button4;
    public string m_lockOnSelectKey = "LockOnSelectKey";

    bool lockOn;

    [SerializeField]
    private float distance = 4.0f; 
    [SerializeField]
    private float polarAngle = 45.0f; 
    [SerializeField]
    private float azimuthalAngle = 45.0f; 

    [SerializeField]
    private float minDistance = 1.0f;
    [SerializeField]
    private float maxDistance = 7.0f;
    [SerializeField]
    private float minPolarAngle = 5.0f;
    [SerializeField]
    private float maxPolarAngle = 75.0f;
    [SerializeField]
    private float CameraXSpeed = 5.0f;
    [SerializeField]
    private float CameraYSpeed = 5.0f;
    [SerializeField]
    private float scrollSensitivity = 5.0f;

    private void Start()
    {
        //カメラの中のロックオンコントローラーを取ってくる
        m_lockOnController = GetComponent<LockOnController>();
        lockOn = true;
    }

    void LateUpdate()
    {   
        var lookAtPos = target.transform.position + offset;

        if (Input.GetKeyDown(m_lockOnKey))
        {
            LockOnTarget = m_lockOnController.LockOnTarget(0);
        }
        
        //カメラのロックオンボタンの処理
        if (Input.GetKey(m_lockOnKey))
        {           
            //ロックオンコントローラーの中のロックオン処理を呼ぶ
            if (Input.GetAxis(m_lockOnSelectKey) > 0.5 && lockOn == true)
            {
                Debug.Log(Input.GetAxis(m_lockOnSelectKey));
                lockOn = false;
                //ロックオン中に右十字ボタンを押されたときの処理
                LockOnTarget = m_lockOnController.LockOnTarget(1);
            }
            else if (Input.GetAxis(m_lockOnSelectKey) < -0.5 && lockOn == true)
            {
                Debug.Log(Input.GetAxis(m_lockOnSelectKey));
                lockOn = false;
                //ロックオン中に左十字ボタンを押されたときの処理
                LockOnTarget = m_lockOnController.LockOnTarget(-1);
            }

            lockOnCameraPosition(LockOnTarget);

        }
        else
        {
            //通常時のカメラの処理を呼ぶ
            updatePosition(lookAtPos);
            updateAngle(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));
            transform.LookAt(lookAtPos);
        }


        if(Input.GetAxis(m_lockOnSelectKey) == 0)
        {
            lockOn = true;
        }
    }

    //通常時カメラの角度を変える処理
    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * CameraXSpeed;
        azimuthalAngle = Mathf.Repeat(x, 360);

        y = polarAngle + y * CameraYSpeed;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    //通常時カメラの位置を変える処理
    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }

    //カメラのロックオン処理のための処理
    void lockOnCameraPosition(GameObject obj)
    {
        //プレイヤーとターゲット中の敵とのベクトルを取りカメラの位置と角度を移動
        Vector3 vec = new Vector3(
            target.transform.position.x + (target.transform.position.x - obj.transform.position.x),
            m_lockOnYPosition,
            target.transform.position.z + (target.transform.position.z - obj.transform.position.z));

        if (vec.magnitude  >= cameraDistance ) {
            transform.position = vec.normalized * cameraDistance;
        }
        else
        {
            transform.position = vec;
        }

        transform.LookAt(obj.transform.position);
    }   
}

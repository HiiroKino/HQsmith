using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject targetObj;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        
        RotateCamera();
    }

    private void MoveCamera()
    {
        // targetの移動量分カメラが移動す
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

    }

    private void RotateCamera()
    {
        // マウスの移動量
        float CameraRotateX = Input.GetAxis("RightHorizontal");
        float CameraRotateY = Input.GetAxis("RightVertical");

        // targetの位置のY軸を中心に、回転する
        transform.RotateAround(targetPos, Vector3.up, CameraRotateX * Time.deltaTime * 200f);

        // カメラの垂直移動        
        transform.RotateAround(targetPos, transform.right, CameraRotateY * Time.deltaTime * 200f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target; // an object to follow
    public Vector3 offset; // offset form the target object

    [SerializeField]
    GameObject m_target;

    public KeyCode m_lockOnKey = KeyCode.Joystick1Button4;

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

    void LateUpdate()
    {
        
        updateAngle(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));
        var lookAtPos = target.transform.position + offset;
        updatePosition(lookAtPos);
        if (Input.GetKey(m_lockOnKey))
        {
            transform.LookAt(m_target.transform.position);
        }
        else
        {
            transform.LookAt(lookAtPos);
        }
    }

    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * CameraXSpeed;
        azimuthalAngle = Mathf.Repeat(x, 360);

        y = polarAngle + y * CameraYSpeed;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}

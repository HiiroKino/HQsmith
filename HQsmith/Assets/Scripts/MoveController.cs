using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = 5f;  //移動速度 m/s

    [SerializeField]
    float m_rotateSpeed = 90f;  //回転速度 度/s

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float value)
    {
        transform.position += transform.forward * m_moveSpeed * value * Time.deltaTime;
    }

    public void Rotate(float value)
    {
        transform.Rotate(0, m_rotateSpeed * value * Time.deltaTime, 0f);
    }
}

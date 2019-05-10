using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    LockOnController m_lockOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameVisible()
    {
        m_lockOn.TargetListAdd(gameObject);
    }

    private void OnBecameInvisible()
    {
        m_lockOn.TargetListRemove(gameObject);
    }
}

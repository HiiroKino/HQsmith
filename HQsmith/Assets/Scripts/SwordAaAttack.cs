using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAaAttack : MonoBehaviour
{
    KatiboshiController m_katiboshiController;
    Collider m_collider;

    ParticleSystem m_particleSystem;
    ParticleSystem.TriggerModule m_triggerModule;
    bool m_hit;

    // Start is called before the first frame update
    void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_triggerModule = m_particleSystem.trigger;
        m_triggerModule.SetCollider(0, m_collider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleTrigger()
    {
        m_katiboshiController.KatibosiGage(40f);
        m_hit = true;
        Invoke("HitEnabled", 1.0f);
    }

    public Collider AaAttackCollider
    {
        set
        {
            m_collider = value;
        }
    }

    public KatiboshiController AaAttackDamage
    {
        set
        {
            m_katiboshiController = value;
        }
    }

    void HitEnabled()
    {
        m_hit = false;
    }
}

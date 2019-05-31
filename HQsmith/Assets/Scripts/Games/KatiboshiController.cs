using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatiboshiController : MonoBehaviour
{
    //敵のユーザーコントローラー＆プレイヤーコントローラー
    [SerializeField]
    UserController m_enemyUserController;
    [SerializeField]
    PlayerController m_enemyPlayerController;

    //カチボシ用のゲージ
    float katibosiCount;

    bool m_guardFlag;

    //カチボシゲージUI
    public Image UIobj;

    //カチボシの最大個数
    public Image[] m_kachiboshi;

    int maxCount;

    //弱攻撃用の変数
    float zyakuAttack;

    //弱攻撃ダメージ用の変数
    float zyakuDamage;

    //強攻撃用の変数
    float kyouAttack;

    //弱攻撃ダメージ用の変数
    float kyouDamage;

    EnemyAI m_enemyAI;

    float DamageInterval;

    Rigidbody m_rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_guardFlag = false;
        m_enemyAI = GetComponent<EnemyAI>();
        DamageInterval = 0f;
        m_rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DamageInterval > 0)
        {
            DamageInterval -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider");
        if (other.tag == "Sword" && m_guardFlag == false && DamageInterval <= 0)
        {
            Debug.Log("Damage");
            DamageInterval = 0.7f;
            Damage();
        }
    }

    //攻撃が当たった時の処理
    public void Damage()
    {
        Debug.Log(m_enemyUserController.m_attackType);
        switch (m_enemyUserController.m_attackType)
        {
            case UserController.AttackType.Attack1:
                m_enemyPlayerController.KatibosiGage(zyakuAttack);
                KatibosiGage(zyakuDamage);
                m_enemyAI.Hit();
                break;
            case UserController.AttackType.Attack2:
                m_enemyPlayerController.KatibosiGage(zyakuAttack);
                KatibosiGage(zyakuDamage);
                m_enemyAI.Hit();
                break;
            case UserController.AttackType.Attack3:
                m_enemyPlayerController.KatibosiGage(zyakuAttack);
                KatibosiGage(zyakuDamage);
                m_enemyAI.Hit();
                break;
            case UserController.AttackType.StrongAttack:
                m_enemyPlayerController.KatibosiGage(kyouAttack);
                KatibosiGage(kyouDamage);
                m_enemyAI.Hit();
                break;
            case UserController.AttackType.KnockBackAttack:
                StartCoroutine("KnockBack");
                m_enemyPlayerController.KatibosiGage(kyouAttack);
                KatibosiGage(kyouDamage);
                m_enemyAI.Hit();
                break;
        }
    }

    //評価ゲージのダメージ、増加処理
    public void KatibosiGage(float damege)
    {
        UIobj.fillAmount += damege;
        if (UIobj.fillAmount > 1f)
        {
            UIobj.fillAmount -= 0f;
            m_kachiboshi[maxCount].enabled = true;
            maxCount++;
        }
        if (UIobj.fillAmount < 0f)
        {
            katibosiCount--;
            UIobj.fillAmount = 1f - UIobj.fillAmount;
        }
    }

    IEnumerator KnockBack()
    {
        for (int i = 0; i < 30; i++)
        {
            transform.position += transform.forward * -3 * Time.deltaTime;
            Debug.Log("KnockBack");
            yield return null;
        }
        yield break;

    }

    public bool GuardFlag
    {
        set
        {
            m_guardFlag = value;
        }
    }
}

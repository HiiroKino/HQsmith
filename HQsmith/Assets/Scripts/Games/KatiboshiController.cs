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

    // Start is called before the first frame update
    void Start()
    {
        m_guardFlag = false;
        m_enemyAI = GetComponent<EnemyAI>();
        DamageInterval = 0f;
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
        if (other.tag == "Sword" && m_guardFlag == false && DamageInterval <= 0)
        {
            DamageInterval = 0.7f;
            Damage();
        }
    }

    //攻撃が当たった時の処理
    public void Damage()
    {
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

    public bool GuardFlag
    {
        set
        {
            m_guardFlag = value;
        }
    }
}

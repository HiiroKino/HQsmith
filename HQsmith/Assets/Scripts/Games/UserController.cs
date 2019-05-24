using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField]
    PlayerController m_playerController;

    public string JoystickLeftHorizontal = "LeftHorizontal";
    public string JoystickLeftVertical = "LeftVertical";

    bool m_guardflag;

    //アニメーション中か判断するフラグ
    bool DuringAnimation;

    //コントローラーで入力された値を入力
    float Horizontal;
    float Vertical;

    //必殺技用のゲージ
    float m_aaGage;

    //弱攻撃用のタイマー
    float m_attackTimer;
    float m_attackIntervalTimer;

    public KeyCode m_attack1Key = KeyCode.Joystick1Button1;    //弱攻撃ボタン
    public KeyCode m_attack2Key = KeyCode.Joystick1Button2;    //強攻撃ボタン
    public KeyCode m_provokeKey = KeyCode.Joystick1Button0;
    public KeyCode m_GuardKey = KeyCode.Joystick1Button5;
    public KeyCode m_AaAttackKey = KeyCode.A;

    public string m_DashKey = "DashKey";   

    //攻撃の種類のenum
    public enum AttackType
    {
        notAttack,
        Attack1,
        Attack2,
        Attack3,
        StrongAttack,
        KnockBackAttack,
        provoke
    }
    public AttackType m_attackType = AttackType.notAttack;

    // Start is called before the first frame update
    void Start()
    {
        m_aaGage = 0;
        DuringAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {   
        //移動処理
        MovePlayer();

        //ガード処理
        GuardFlag();

        //ダッシュ処理
        DashFlag();

        //タイマー処理
        AttackTimer();

        if (DuringAnimation == false && m_attackIntervalTimer <= 0)
        {
            AttackButton();
        }
    }

    //移動処理のための関数
    public void MovePlayer()
    {
        //移動の操作処理
        Horizontal = Input.GetAxis(JoystickLeftHorizontal);
        m_playerController.Horizontal = Horizontal;
        Vertical = Input.GetAxis(JoystickLeftVertical);
        m_playerController.Vertical = Vertical;
    }

    //攻撃用のボタン処理
    void AttackButton()
    {
        //弱攻撃ボタン処理
        if (Input.GetKeyDown(m_attack1Key))
        {
            DuringAnimation = true;
            Attack();
        }
        //強攻撃ノックバック攻撃ボタン処理
        if (Input.GetKeyDown(m_attack2Key))
        {
            if (m_guardflag == false)
            {
                DuringAnimation = true;
                m_attackType = AttackType.StrongAttack;
            }
            else
            {
                DuringAnimation = true;
                m_attackType = AttackType.KnockBackAttack;
            }
            Attack();
        }
        //挑発ぼたん処理
        if (Input.GetKeyDown(m_provokeKey))
        {
            DuringAnimation = true;
            m_attackType = AttackType.provoke;
            Attack();
        }   

        //AAアタックのボタン処理
        if (Input.GetKeyDown(m_AaAttackKey) && m_playerController.m_aaGageState == PlayerController.AAGageState.None)
        {
            AaAttack();
        }
    }

    //攻撃用のタイマーの処理
    void AttackTimer()
    {
        //弱攻撃のための時間
        if (m_attackTimer > 0)
        {
            m_attackTimer -= Time.deltaTime;
        }
        else if (m_attackTimer <= 0)
        {
            m_attackType = AttackType.notAttack;
        }

        //攻撃用のインターバルタイマー
        if (m_attackIntervalTimer > 0)
        {
            m_attackIntervalTimer -= Time.deltaTime;
        }
    }

    //攻撃処理のための関数
    void Attack()
    {
        
        if (m_guardflag == false) {
            if(m_attackType == AttackType.notAttack)
            {
                m_attackType = AttackType.Attack1;
            }
            if (m_attackType == AttackType.Attack1)
            {
                m_attackType = AttackType.Attack2;           
                m_playerController.PlayAnimation("Attack1", 1.5f);
            }
            else if (m_attackType == AttackType.Attack2)
            {
                m_attackType = AttackType.Attack3;
                m_playerController.PlayAnimation("Attack2",1.5f);
            }
            else if (m_attackType == AttackType.Attack3)
            {
                m_attackType = AttackType.Attack1;
                m_playerController.PlayAnimation("Attack3",1.5f);
            }
            else if (m_attackType == AttackType.StrongAttack)
            {
                m_attackType = AttackType.notAttack;
                m_playerController.PlayAnimation("StrongAttack",1.5f);
            }
            else if (m_attackType == AttackType.provoke)
            {
                m_attackType = AttackType.notAttack;
                m_playerController.PlayAnimation("Provoke");
            }
        }
        if (m_attackType == AttackType.KnockBackAttack)
        {
            m_attackType = AttackType.notAttack;
            m_playerController.PlayAnimation("KnockbackAttack");
        }
    }   

    public void AaAttack()
    {
        if (m_playerController.m_aaGageState == PlayerController.AAGageState.one)
        {
            m_playerController.AaAttack("one");
        }
        else if (m_playerController.m_aaGageState == PlayerController.AAGageState.two)
        {
            m_playerController.AaAttack("two");
        }
    }

    //ガード処理
    public void GuardFlag()
    {
        if (Input.GetKey(m_GuardKey))
        {
            //Debug.Log("ガード中ですよー");
            m_guardflag = true;
            m_playerController.GuardFlag(m_guardflag);
        }
        else
        {
            m_guardflag = false;
            m_playerController.GuardFlag(m_guardflag);
        }
    }

    //ダッシュ処理
    public void DashFlag()
    {
        if(Input.GetAxis(m_DashKey) > 0)
        {
            m_playerController.DashFlag(true);
        }
        else
        {
            m_playerController.DashFlag(false);
        }
    }  

    void OnAnimationFinished()
    {
        DuringAnimation = false;
        m_attackTimer = 3f;
        m_attackIntervalTimer = 0.5f;
    }
}

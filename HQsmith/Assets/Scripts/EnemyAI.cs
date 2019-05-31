using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    EnemyUI enemyUI;

    public enum ActionType
    {
        Attack,       //攻撃
        Escape,       //回避
        Guard,        //ガード
        Provocation,  //挑発
        TakeABreak,   //距離を取る
        GetClose,     //接近する
    }

    public enum AttackType
    {
        Attack1,
        Attack2,
        Attack3,
        StrongAttack,
        KnockBackAttack,
    }
    public AttackType m_attackType = AttackType.Attack1;

    float m_comboTimer;
    float m_attackIntervalTimer;


    [SerializeField]
    GameManager gameManager;


    Vector3 vec;

    public ActionType m_actionType = ActionType.GetClose;

    MoveController m_moveController;

    KatiboshiController m_katibosiController;

    //武器用のコライダー
    [SerializeField]
    Collider WeponColider;

    //攻撃などのアニメーション中か判定する
    bool DuringAnimation;

    NavMeshAgent m_navMeshAgent;

    SimpleAnimation m_simpleAnimation;

    int m_chooseAction = 0;
    int m_choosePos = 0;

    Transform m_position;

    float StepIntervalTimer = 0f;
    float actionIntervalTimer = 0f;

    [SerializeField]
    GameObject m_target;  //目標

    UserController m_userController;

    // Start is called before the first frame update
    void Start()
    {
        m_simpleAnimation = GetComponent<SimpleAnimation>();
        m_moveController = this.GetComponent<MoveController>();
        m_navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_position = this.GetComponent<Transform>();
        m_userController = m_target.GetComponent<UserController>();
        m_katibosiController = GetComponent<KatiboshiController>();
        m_moveController.DuringAnimation = false;
        DuringAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_actionType == ActionType.Guard)
        {
            m_katibosiController.GuardFlag = true;
        }
        else
        {
            m_katibosiController.GuardFlag = false;
        }
        //if(!DuringAnimation && m_attackIntervalTimer > 0)
        //{
        //    //Debug.Log("アイドル状態だよー");
        //    m_simpleAnimation.Play("Idle");
        //}
        if (!DuringAnimation)
        {
            //Debug.Log("走ってるよ～");
            m_simpleAnimation.Play("Run");
        }

        
        if (actionIntervalTimer >= 0)
        {
            actionIntervalTimer -= Time.deltaTime;
        }
        
        //タイマー関連の処理
        Timer();
        if (DuringAnimation == false && m_attackIntervalTimer <= 0)
        {

            switch (m_actionType )
            {
                case ActionType.Attack:
                    Attack();
                    break;
                case ActionType.Escape:
                    Escape();
                    break;
                case ActionType.Guard:
                    Guard();
                    break;
                case ActionType.Provocation:
                    Provocation();
                    break;
                case ActionType.TakeABreak:
                    TakeABreak();
                    break;
                case ActionType.GetClose:
                    GetClose();
                    break;
            }
        }
    }

    void Timer()
    {

        //ステップインターバルタイマー
        if (StepIntervalTimer > 0)
        {
            StepIntervalTimer -= Time.deltaTime;
        }

        //コンボ用タイマー処理
        if(m_comboTimer > 0)
        {
            m_comboTimer -= Time.deltaTime;
        }
        else
        {
            m_attackType = AttackType.Attack1;
        }

        //攻撃のインターバル用のタイマー処理
        if (m_attackIntervalTimer > 0)
        {
            m_attackIntervalTimer -= Time.deltaTime;
        }
        
    }

    void Attack()
    {
        if (!DuringAnimation && m_attackIntervalTimer <= 0)
        {
            //Debug.Log("EnemyAttack!!!");
            actionIntervalTimer = 1.0f;
            ChangeActionType(ActionType.GetClose);
            m_moveController.DuringAnimation = true;
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    if (m_attackType == AttackType.Attack1)
                    {
                        //Debug.Log("攻撃１だよ～");
                        m_simpleAnimation.CrossFade("Attack1", 0.3f);
                        m_attackType = AttackType.Attack2;

                    }
                    else if (m_attackType == AttackType.Attack2)
                    {
                        //Debug.Log("攻撃2だよ～");
                        m_simpleAnimation.CrossFade("Attack2", 0.3f);
                        m_attackType = AttackType.Attack3;
                    }
                    else if (m_attackType == AttackType.Attack3)
                    {
                        //Debug.Log("攻撃３だよ～");
                        m_simpleAnimation.CrossFade("Attack3", 0.3f);
                        m_attackType = AttackType.Attack1;
                    }
                    DuringAnimation = true;
                    break;
                case 1:
                    m_attackType = AttackType.StrongAttack;
                    m_simpleAnimation.CrossFade("StrongAttack", 0.3f);
                    DuringAnimation = true;
                    break;
                case 2:
                    m_attackType = AttackType.KnockBackAttack;
                    m_simpleAnimation.CrossFade("KnockBackAttack", 0.3f);
                    DuringAnimation = true;
                    break;

            }
        }
        
    }

    void Escape()
    {
        if (StepIntervalTimer <= 0 && !DuringAnimation )
        {
            //Debug.Log("Escape!!!");
            actionIntervalTimer = 1.0f;
            StepIntervalTimer = 0.7f;
            switch (m_choosePos = Random.Range(0, 2))
            {
                case 0:
                    DuringAnimation = true;
                    //Debug.Log("右ステップだよー");
                    m_simpleAnimation.Play("RightStep");
                    break;
                case 1:
                    DuringAnimation = true;
                    //Debug.Log("左ステップだよー");
                    m_simpleAnimation.Play("LeftStep");
                    break;
                default:
                    break;
            }
        }
        ChangeActionType(ActionType.GetClose);
    }
    void Guard()
    {
        if (!DuringAnimation)
        {
            //Debug.Log("ガードだよー");
            m_simpleAnimation.CrossFade("Guard", 0.1f);
            actionIntervalTimer = 1.0f;
            m_navMeshAgent.SetDestination(this.transform.position);
            ChangeActionType(ActionType.GetClose);
        }
    }
    void Provocation()
    {

    }
    void TakeABreak()
    {
        //Debug.Log("Take A Break!!!");
        actionIntervalTimer = 1.0f;
        m_position.position += new Vector3(0, 0, 10);
        ChangeActionType(ActionType.GetClose);
    }
    void GetClose()
    {
        //m_navMeshAgent.isStopped = false;


        //Navmeshの経路探索準備が出来ているなら
        if (m_navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            //自動でプレイヤーに接近する
            m_navMeshAgent.SetDestination(m_target.transform.position);
        }

        //近付いてるときに攻撃されたら、避ける、ガードをする、そのまま突っ込む
        if (m_userController.m_attackType != UserController.AttackType.notAttack && actionIntervalTimer <= 0)
        {
            m_chooseAction = Random.Range(0, 100);
            if(m_chooseAction >= 95)
            {
                ChangeActionType(ActionType.Guard);
            }
            else if(m_chooseAction  >= 90)
            {
                ChangeActionType(ActionType.Escape);
            }
            else if (m_chooseAction >= 80)
            {
                ChangeActionType(ActionType.TakeABreak);
            }
        }
        //近付いてる最中に挑発

        //近付いたら攻撃
        if (Vector3.Distance(transform.position,m_target.transform.position) < 5f)
        {
            //ステートを変える
            ChangeActionType(ActionType.Attack);
        }
    }

    void ChangeActionType(ActionType type)
    {
        m_actionType = type;
        switch(m_actionType){
                case ActionType.Attack:
                Debug.Log("isStopped true");
                m_navMeshAgent.isStopped = true;
                break;
                case ActionType.Escape:
            break;
                case ActionType.Guard:
            break;
                case ActionType.Provocation:
                    Provocation();
            break;
                case ActionType.TakeABreak:
            break;
                case ActionType.GetClose:
                
                break;
        }
    }

    void StartAttack()
    {
        WeponColider.enabled = true;
    }
    void EndAttack()
    {
        WeponColider.enabled = false;
    }
    void OnAnimationFinished()
    {
        m_navMeshAgent.isStopped = false;
        DuringAnimation = false;
        m_comboTimer = 2f;
        m_actionType = ActionType.GetClose;
        m_moveController.DuringAnimation = false;
        m_attackIntervalTimer = 1f;
        m_navMeshAgent.SetDestination(m_target.transform.position);
    }

    public void Hit()
    {
        WeponColider.enabled = false;
        DuringAnimation = true;
        
        m_simpleAnimation.Play("Hit");
        
    }

    void StepStart()
    {

    }

    void StepEnd()
    {
        DuringAnimation = false;
    }
    
    void InstantiateEffect()
    {

    }

    void FootR()
    {

    }

    void FootL()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum ActionType
    {
        Attack,       //攻撃
        Escape,       //回避
        Guard,        //ガード
        Provocation,  //挑発
        TakeABreak,   //距離を取る
        GetClose,     //接近する
    }

    enum AttackType
    {
        Attack1,
        Attack2,
        Attack3,
    }
    AttackType m_attackType = AttackType.Attack1;

    float m_comboTimer;

    ActionType m_actionType = ActionType.GetClose;

    MoveController m_moveController;

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
        DuringAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_userController.m_attackType);
        if(actionIntervalTimer >= 0)
        {
            actionIntervalTimer -= Time.deltaTime;
        }
        
        //タイマー関連の処理
        Timer();

        switch (m_actionType)
        {
            case ActionType.Attack:
                if (DuringAnimation == false)
                {
                    Attack();
                }
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

    }

    void Attack()
    {
        Debug.Log("EnemyAttack!!!");
        actionIntervalTimer = 1.0f;
        ChangeActionType(ActionType.GetClose);
        m_moveController.DuringAnimation = true;
        switch ( Random.Range(0, 2))
        {
            case 0:
                if (m_attackType == AttackType.Attack1)
                {
                    m_simpleAnimation.CrossFade("Attack1", 0.3f);
                    m_attackType = AttackType.Attack2;
                    
                }
                if (m_attackType == AttackType.Attack2)
                {
                    m_simpleAnimation.CrossFade("Attack2", 0.3f);
                    m_attackType = AttackType.Attack3;
                }
                if (m_attackType == AttackType.Attack3)
                {
                    m_simpleAnimation.CrossFade("Attack3", 0.3f);
                    m_attackType = AttackType.Attack1;
                }
                DuringAnimation = true;
                break;
            case 1:
                m_simpleAnimation.CrossFade("StrongAttack", 0.3f);
                DuringAnimation = true;
                break;
            case 2:
                m_simpleAnimation.CrossFade("KnockBackAttack", 0.3f);
                DuringAnimation = true;
                break;

        }
        
    }

    void Escape()
    {
        if (StepIntervalTimer <= 0)
        {
            Debug.Log("Escape!!!");
            actionIntervalTimer = 1.0f;
            StepIntervalTimer = 0.7f;
            switch (m_choosePos = Random.Range(0, 2))
            {
                case 0:
                    m_position.position += new Vector3(5, 0);                   
                    break;
                case 1:
                    m_position.position += new Vector3(-5, 0);
                    break;
                default:
                    break;
            }
        }
        ChangeActionType(ActionType.GetClose);
    }
    void Guard()
    {
        Debug.Log("Guard!!!");
        actionIntervalTimer = 1.0f;
        m_navMeshAgent.SetDestination(this.transform.position);
        ChangeActionType(ActionType.GetClose);
    }
    void Provocation()
    {

    }
    void TakeABreak()
    {
        Debug.Log("Take A Break!!!");
        actionIntervalTimer = 1.0f;
        m_position.position += new Vector3(0, 0, 10);
        ChangeActionType(ActionType.GetClose);
    }
    void GetClose()
    {
        //Navmeshの経路探索準備が出来ているなら
        if(m_navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
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
        DuringAnimation = false;
        m_comboTimer = 2f;
        m_actionType = ActionType.GetClose;
        m_moveController.DuringAnimation = false; 
    }
    
    void InstantiateEffect()
    {

    }
}

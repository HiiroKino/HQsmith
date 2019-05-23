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

    ActionType m_actionType = ActionType.GetClose;

    MoveController m_moveController;

    NavMeshAgent m_navMeshAgent;

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
        m_moveController = this.GetComponent<MoveController>();
        m_navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_position = this.GetComponent<Transform>();
        m_userController = m_target.GetComponent<UserController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_chooseAction);
        //Debug.Log(m_userController.m_attackType);
        if(actionIntervalTimer >= 0)
        {
            actionIntervalTimer -= Time.deltaTime;
        }


        //ステップインターバルタイマー
        if (StepIntervalTimer > 0)
        {
            StepIntervalTimer -= Time.deltaTime;
        }

        switch (m_actionType)
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

    void Attack()
    {
        Debug.Log("EnemyAttack!!!");
        actionIntervalTimer = 1.0f;
        ChangeActionType(ActionType.GetClose);
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
            else if(m_chooseAction  >= 85)
            {
                ChangeActionType(ActionType.Escape);
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
}

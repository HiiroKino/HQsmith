using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //ユーザーコントローラーから送られてきた値をもらうための関数
    float m_horizontal;
    float m_vertical;

    //敵のプレイヤーコントローラー
    [SerializeField]
    PlayerController m_enemyPlayerController;
    //敵のユーザーコントローラー
    [SerializeField]
    UserController m_enemyUsetController;

    //ガード用のフラグ
    bool m_guardFlag;
    
    float MoveSpeed = 3f;
    float StepIntervalTimer = 0f;

    //AAゲージ用の変数
    float m_aaGage;
    float m_maxAaGage = 200;

    //ガードゲージ用の変数
    float m_guardGage;
    float m_maxGuardGage = 100;

    //カチボシ用のゲージ
    float katibosigage;
    float katibosiCount;

    //攻撃のダメージ用の変数
    [SerializeField]
    float zyakuDamage = 4f;
    [SerializeField]
    float kyouDamage = 7f;

    //攻撃時のカチボシゲージを増やすための変数
    [SerializeField]
    float zyakuAttack = 4f;
    [SerializeField]
    float kyouAttack = 7f;

    //カチボシゲージUI
    Image katibosimeter;

    //防御ゲージUI
    Image Guardmeter;

    //コンボ用のカウント
    float ComboCount;

    public bool _isRendered = true;

    public enum AAGageState
    {
        None,
        one,
        two
    }
    public AAGageState m_aaGageState = AAGageState.None;

    // Start is called before the first frame update
    void Start()
    {
        ComboCount = 0;
        katibosigage = 0;
        katibosiCount = 0;
        StartCoroutine("AaGage");



    }

    // Update is called once per frame
    void Update()
    {
        //移動処理用関数
        MovePlayer();
        
        //ステップインターバルタイマー
        if(StepIntervalTimer > 0)
        {
            StepIntervalTimer -= Time.deltaTime; 
        }

        //アルティメットアタック用
        if(m_aaGage >= 200)
        {
            m_aaGageState = AAGageState.two;
        }
        else if(m_aaGage >= 100)
        {
            m_aaGageState = AAGageState.one;
        }
    }

    //移動処理
    public void MovePlayer()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * m_vertical + Camera.main.transform.right * m_horizontal;

        if (m_guardFlag == false)
        {
            //通常移動処理
            // 移動方向に進ませる
            transform.position += moveForward * MoveSpeed * Time.deltaTime;

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveForward), 0.3f);
            }
        }
        else if(StepIntervalTimer <= 0 && m_guardFlag )
        {
            //ステップ処理
            if (Mathf.Abs(moveForward.x) >= 0.5 || Mathf.Abs(moveForward.z) >= 0.5)
            {
                Debug.Log("ステップ");
                //ステップのインターバル
                StepIntervalTimer = 0.7f;
                //移動方向にステップ
                transform.position += moveForward * 30f * Time.deltaTime;
            }
        }
    }

    //ガード処理
    public void GuardFlag(bool value)
    {
        m_guardFlag = value;
    }

    //ダッシュ処理
    public void DashFlag(bool value)
    {
        if(value == true)
        {
            MoveSpeed = 20f;
        }
        else
        {
            MoveSpeed = 3f;
        }
    }

    public void PlayAnimation(string str)
    {
        Debug.Log(str);
    }

    public void AaAttack(string str)
    {
        Debug.Log(str);
        m_aaGage = 0;
        m_aaGageState = AAGageState.None;
    }

    public float Horizontal
    {
        set
        {
            m_horizontal = value;
        }
    }

    public float Vertical
    {
        set
        {
            m_vertical = value;
        }
    }

    //カメラにプレイヤーが写っているのかを判定するメソッド
    private void OnWillRenderObject()
    {
        if (Camera.current.tag == "PlayerCamera")
        {
            _isRendered = true;
        }
        else
        {
            _isRendered = false;
        }
    }

    //AAゲージの自動増加処理
    IEnumerator AaGage()
    {
        m_aaGage += 1;
        yield return new WaitForSeconds(1f);
    }

    //ガードゲージの自動増加処理
    IEnumerator GuardGage()
    {
        m_guardGage += 1;
        if(m_guardGage > m_maxGuardGage)
        {
            m_guardGage = m_maxGuardGage;
        }
        yield return new WaitForSeconds(1f);
    }

    //評価ゲージのダメージ、増加処理
    public void KatibosiGage(float damege)
    {
        damege *= (1 + ComboCount / 10);
        katibosigage += damege;
        if(katibosigage > 100)
        {
            katibosiCount++;
            katibosigage = 0;
        }
        if (katibosigage < 0)
        {
            katibosiCount--;
            katibosigage = 100 - katibosigage;
        }
    }

    //ガードゲージの減少処理
    public void GuardGage(float damege)
    {
        m_guardGage -= damege;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sword")
        {
            switch (m_enemyUsetController.m_attackType)
            {
                case UserController.AttackType.Attack1:
                    ComboCount++;
                    m_enemyPlayerController.KatibosiGage(zyakuAttack);
                    KatibosiGage(zyakuDamage);
                    break;
                case UserController.AttackType.Attack2:
                    ComboCount++;
                    m_enemyPlayerController.KatibosiGage(zyakuAttack);
                    KatibosiGage(zyakuDamage);
                    break;
                case UserController.AttackType.Attack3:
                    ComboCount++;
                    m_enemyPlayerController.KatibosiGage(zyakuAttack);
                    KatibosiGage(zyakuDamage);
                    break;
                case UserController.AttackType.StrongAttack:
                    ComboCount++;
                    m_enemyPlayerController.KatibosiGage(kyouAttack);
                    KatibosiGage(kyouDamage);
                    break;
                case UserController.AttackType.KnockBackAttack:
                    ComboCount++;
                    m_enemyPlayerController.KatibosiGage(kyouAttack);
                    KatibosiGage(kyouDamage);
                    break;
            }
        }
    }
}

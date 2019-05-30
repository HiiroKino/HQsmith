using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //ユーザーコントローラーから送られてきた値をもらうための関数
    float m_horizontal;
    float m_vertical;   
    
    //シンプルアニメーション
    SimpleAnimation m_simpleAnimation;
    //敵のプレイヤーコントローラー
    [SerializeField]
    PlayerController m_enemyPlayerController;
    //敵のユーザーコントローラー
    [SerializeField]
    UserController m_enemyUsetController;

    [SerializeField]
    BoxCollider WeponColider;

    //アニメーション中じゃないか判断
    bool DuringAnimation;
    //ガード用のフラグ
    bool m_guardFlag;
    
    float MoveSpeed = 3f;
    float StepIntervalTimer = 0f;

    //AAゲージUI
    public Image aaGauge_1;
    //public Image aaGauge_2;

    //AAゲージ用の変数
    float m_aaGage;
    float m_maxAaGage = 100;

    //AAゲージ自動増加量
    [SerializeField]
    float autoAAgauge = 0.05f;

    //AAゲージの自動上昇制御
    private bool AAgaugeManager;

    //AAゲージ増加量
    [SerializeField]
    float AAgaugeAmount = 0.05f;


    //ガードゲージ用の変数
    public Image m_guardGage;
    float m_maxGuardGage = 100;
    private bool GuardGaugeManager;
    float autoGuardGauge = 0.05f;

    //カチボシ用のゲージ
    float katibosiCount;

    //攻撃のダメージ用の変数
    [SerializeField]
    float zyakuDamage = 4f;
    [SerializeField]
    float kyouDamage = 7f;

    //攻撃時のカチボシゲージを増やすための変数
    [SerializeField]
    float zyakuAttack = 0.4f;
    [SerializeField]
    float kyouAttack = 0.7f;

    //カチボシゲージUI
    public Image UIobj;

    //カチボシの最大個数
    public Image[] m_kachiboshi;

    //カチボシゲージ最大到達回数
    public int maxCount;

    //カチボシゲージ増加量
    [SerializeField]
    public float kachiboshigaugeAmount = 0.05f;
    
    public bool m_isRendered = true;

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
        DuringAnimation = false;
        WeponColider.enabled = false;
        katibosiCount = 0;
        //StartCoroutine("AaGage");
        m_simpleAnimation = GetComponent<SimpleAnimation>();
        UIobj.fillAmount = 0f;
        aaGauge_1.fillAmount = 0f;
        m_kachiboshi[0].enabled = false;
        m_kachiboshi[1].enabled = false;
        m_kachiboshi[2].enabled = false;
        maxCount = 0;
        m_guardGage.fillAmount = 0f;
        AAgaugeManager = true;
        GuardGaugeManager = true;

    }

    // Update is called once per frame
    void Update()
    {
        //移動処理用関数
        if (DuringAnimation == false)
        {
            MovePlayer();
        }
        
        //ステップインターバルタイマー
        if(StepIntervalTimer > 0)
        {
            StepIntervalTimer -= Time.deltaTime; 
        }

        
        //UIobj.fillAmount += Time.deltaTime * 0.1f;

        //カチボシゲージがマックスになったらリセットしてカチボシを１つ増やす
        if(UIobj.fillAmount >= 1)
        {
            UIobj.fillAmount = 0f;
            m_kachiboshi[maxCount].enabled = true;
            maxCount++;

            //カチボシを３つ獲得したらリザルト画面へ
            if(maxCount == 2)
            {

            }
        }


        //ガードゲージ自動増加処理
        if(m_guardGage.fillAmount >= 1f)
        {
            GuardGaugeManager = false;

            if(m_guardGage.fillAmount < 1f)
            {
                GuardGaugeManager = true;
            }
        }
        
        if (GuardGaugeManager == true)
        {
            m_guardGage.fillAmount += Time.deltaTime * autoGuardGauge;
        }

        //AAゲージ自動増加処理
        if (AAgaugeManager == true)
        {
            aaGauge_1.fillAmount += Time.deltaTime * autoAAgauge;

            if(aaGauge_1.fillAmount >= 1f)
            {
                AaAttack("None");
            }

        }

        

        //アルティメットアタック用
        /*if(m_aaGage >= 1f)
        {
            m_aaGageState = AAGageState.two;
        }
        else if(m_aaGage >= 0.5f)
        {
            m_aaGageState = AAGageState.one;
        }*/

        //ガード用処理
        if (m_guardFlag == true)
        {
            Guard();
        }

    }

    //移動処理
    public void MovePlayer()
    {
        if (m_guardFlag == false) {
            if (m_vertical == 0 && m_horizontal == 0) {
                //何もしていない時のアニメーション処理
                m_simpleAnimation.CrossFade("Idle",0.3f);
            }
            else {
                //走るときのアニメーション処理
                m_simpleAnimation.CrossFade("Run", 0.3f);
            }
        }
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
    
    void Guard()
    {
        m_simpleAnimation.CrossFade("Guard",0.3f);
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

    //アニメーション再生
    public void PlayAnimation(string str , float x = 1)
    {
        DuringAnimation = true;
        m_simpleAnimation.GetState(str).speed = x;
        Debug.Log("攻撃アニメーション中だよ～" + " " + str);
        m_simpleAnimation.CrossFade(str, 0.4f);
    }

    //リセット
    public void AaAttack(string str)
    {
        Debug.Log(str);
        aaGauge_1.fillAmount = 0f;
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
            m_isRendered = true;
        }
        else
        {
            m_isRendered = false;
        }
    }

    /*//AAゲージの自動増加処理
    IEnumerator AaGage()
    {
        aaGauge_1.fillAmount += 0.01f;
        Debug.Log("自動増加しているよ");
        yield return new WaitForSeconds(1f);
    }*/

    /*//ガードゲージの自動増加処理
    IEnumerator GuardGage()
    {
        m_guardGage += 1;
        if(m_guardGage > m_maxGuardGage)
        {
            m_guardGage = m_maxGuardGage;
        }
        yield return new WaitForSeconds(1f);
    }*/

    //評価ゲージのダメージ、増加処理
    public void KatibosiGage(float damege)
    {
        damege = kachiboshigaugeAmount;
        UIobj.fillAmount += damege;
        if(UIobj.fillAmount > 1f)
        {
            UIobj.fillAmount -= 0f;
        }
        if (UIobj.fillAmount < 0f)
        {
            katibosiCount--;
            UIobj.fillAmount = 1f - UIobj.fillAmount;
        }
    }

    //ガードゲージの減少処理
    public void GuardGage(float damege)
    {
        m_guardGage.fillAmount -= damege;
    }

    //ボックスコライダーに攻撃が当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sword" && m_guardFlag == false)
        {
            Damage();
        }
        else
        {
            GuardGage(0.1f);
        }
    }

    //AAゲージ増加処理
    public void AAgaugeMethod(float AAdamage)
    {
        AAdamage = AAgaugeAmount;
        aaGauge_1.fillAmount = AAdamage;
        if(aaGauge_1.fillAmount < 0f)
        {
            aaGauge_1.fillAmount = 1f - aaGauge_1.fillAmount;
        }
    }

    //攻撃が当たった時の処理
    void Damage()
    {
        switch (m_enemyUsetController.m_attackType)
        {
            case UserController.AttackType.Attack1:
                m_enemyPlayerController.KatibosiGage(zyakuAttack);
                KatibosiGage(zyakuDamage);
                AAgaugeMethod(zyakuDamage);
                break;
            case UserController.AttackType.Attack2:
                m_enemyPlayerController.KatibosiGage(zyakuAttack);
                KatibosiGage(zyakuDamage);
                AAgaugeMethod(zyakuDamage);
                break;
            case UserController.AttackType.Attack3:
                m_enemyPlayerController.KatibosiGage(zyakuAttack);
                KatibosiGage(zyakuDamage);
                AAgaugeMethod(zyakuDamage);
                break;
            case UserController.AttackType.StrongAttack:
                m_enemyPlayerController.KatibosiGage(kyouAttack);
                KatibosiGage(kyouDamage);
                AAgaugeMethod(kyouDamage);
                break;
            case UserController.AttackType.KnockBackAttack:
                m_enemyPlayerController.KatibosiGage(kyouAttack);
                KatibosiGage(kyouDamage);
                AAgaugeMethod(kyouDamage);
                break;
        }
    }

    //コライダー用のアニメーションイベント
    //---------------------------------------------
    void StartAttack()
    {
        WeponColider.enabled = true;
    }

    void EndAttack()
    {
        WeponColider.enabled = false;
    }
    //---------------------------------------------

    void OnAnimationFinished()
    {
        DuringAnimation = false;
    }

    //足音用のアニメーションイベント
    void FootR()
    {

    }
    void FootL()
    {

    }
}

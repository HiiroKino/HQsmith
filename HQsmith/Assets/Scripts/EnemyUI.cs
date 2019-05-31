using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{

    public GameObject enemyCanvas;


    //カチボシゲージ
    public Image kachiboshiGauge;

    float autoKachiboshiGauge ;

    bool kachiboshiGaugeManager;

    //AAゲージ
    public Image aaGauge;

    float autoAAGauge = 0.01f;

    bool aaGaugeManager;

    //防御ゲージ
    private float guardGauge = 1f;

    float autoGuardGauge = 0.03f;

    bool guardGaugeManager;

    //カチボシ
    public Animator[] m_kachiboshiAnimator;
    private int getKachiboshiCount;

    // Start is called before the first frame update
    void Start()
    {
        enemyCanvas = transform.parent.gameObject;
        kachiboshiGauge.fillAmount = 0f;
        kachiboshiGaugeManager = true;
        aaGauge.fillAmount = 0f;
        aaGaugeManager = true;
        guardGauge = 1f;
        guardGaugeManager = false;
        getKachiboshiCount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        enemyCanvas.transform.LookAt(GameObject.Find("MainCamera").transform);


        //AAゲージ自動増加処理
        aaGauge.fillAmount += Time.deltaTime * autoAAGauge;

        //ガードゲージ自動増加処理
        if (guardGauge < 1f)
        {
            guardGaugeManager = true;
        }

        if (guardGaugeManager == true)
        {
            guardGauge += Time.deltaTime * autoGuardGauge;
        }

        //カチボシゲージが満タンになったら
        if(kachiboshiGauge.fillAmount >= 1f)
        {
            KachiboshigaugeReset();
        }
        
    }

    //AAゲージのリセット
    public void AAGaougeReset()
    {
        aaGauge.fillAmount = 0f;
        aaGaugeManager = true;
    }

    //カチボシゲージのリセットとカチボシの増加処理
    public void KachiboshigaugeReset()
    {
        if(getKachiboshiCount > 0)
        {
            kachiboshiGauge.fillAmount = 0f;

            if (m_kachiboshiAnimator.Length > 0)
            {
                m_kachiboshiAnimator[getKachiboshiCount].SetBool("On", true);
                getKachiboshiCount++;
                Debug.Log("kachiboshi");
            }
        }
    }


    //攻撃時のカチボシゲージ増加処理
    public void KachiboshigaugeMethod(float kachiboshiDamage)
    {

        kachiboshiGauge.fillAmount += kachiboshiDamage;

        
    }

    //攻撃時のAAゲージ増加処理
    public void AAgaugeMethod(float AAdamage)
    {
        aaGauge.fillAmount += AAdamage;
    }

    //l攻撃を受けた際の防御ゲージ減少処理
    public void GuardGaugeMethod(float guardDamage)
    {
        guardGauge -= guardDamage;
    }
}

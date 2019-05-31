using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{

    [SerializeField]
    Image Cursor;

    [SerializeField]
    AudioClip CursorSE;
    [SerializeField]
    AudioClip SystemSE;

    AudioSource audioSource;
    RectTransform m_this_position;

    private int cursorPosition = 0;
    bool isWait = false;
    public bool isActive = false;

    private int winner;

    [SerializeField]
    Image win;

    [SerializeField]
    Image lose;

    // Start is called before the first frame update
    void Start()
    {
        win.enabled = false;
        lose.enabled = false;
        winner = GameManager.Winner();
        winnerPreview(winner);

        audioSource = GetComponent<AudioSource>();
        m_this_position = Cursor.GetComponent<RectTransform>();
        m_this_position.anchoredPosition = new Vector2(-350, 50);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("LeftHorizontal");

        Debug.Log(x);

        if (!isWait)
        {
            if (x > 0)
            {
                cursorPosition++;
                MovePositions();
                StartCoroutine("Wait");
            }
            if (x < 0)
            {
                cursorPosition--;
                MovePositions();
                StartCoroutine("Wait");
            }
            if (Input.GetKey(KeyCode.Joystick1Button2))
            {
                audioSource.PlayOneShot(SystemSE);
                StartCoroutine("Checking");
            }

        }
    }

    void winnerPreview(int i)
    {
        switch (i)
        {
            case 0:
                win.enabled = true;
                break;
            case 1:
                lose.enabled = true;
                break;
            default:
                break;
        }
    }

    private void MovePositions()
    {
        if (cursorPosition < 0)
        {
            cursorPosition = 1;
        }

        if (cursorPosition > 1)
        {
            cursorPosition = 0;
        }

        switch (cursorPosition)
        {
            case 0:
                m_this_position.anchoredPosition = new Vector2(-350, 50);
                audioSource.PlayOneShot(CursorSE);
                break;
            case 1:
                m_this_position.anchoredPosition = new Vector2(110, 50);
                audioSource.PlayOneShot(CursorSE);
                break;
            default:
                break;
        }
    }

    private void MoveScenes()
    {
        switch (cursorPosition)
        {
            case 0:
                SceneManager.LoadScene("Sample_kino");  //PlayScene
                break;
            case 1:
                SceneManager.LoadScene("TitleScene");
                break;
            default:
                break;
        }
    }

    private IEnumerator Wait()
    {
        if (!isWait)
        {
            isWait = true;
            yield return new WaitForSeconds(0.2f);
            isWait = false;
        }

    }

    private IEnumerator Checking()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying)
            {
                MoveScenes();
                break;
            }
        }
    }

}

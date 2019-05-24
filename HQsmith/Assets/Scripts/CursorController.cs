using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
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
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_this_position = this.GetComponent<RectTransform>();
        m_this_position.anchoredPosition = new Vector2(-190, -100);
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("LeftVertical");

        if (!isWait && isActive)
        {
            if (y < 0)
            {
                cursorPosition++;
                MovePositions();
                StartCoroutine("Wait");
            }
            if (y > 0)
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
                m_this_position.anchoredPosition = new Vector2(-190, -100);
                audioSource.PlayOneShot(CursorSE);
                break;
            case 1:
                m_this_position.anchoredPosition = new Vector2(-190, -170);
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
                Application.Quit();
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

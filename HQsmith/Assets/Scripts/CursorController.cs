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
        m_this_position.anchoredPosition = new Vector2(-125, -110);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWait && isActive)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                cursorPosition++;
                MovePositions();
                StartCoroutine("Wait");
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                cursorPosition--;
                MovePositions();
                StartCoroutine("Wait");
            }
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                audioSource.PlayOneShot(SystemSE);
                StartCoroutine("Checking");
            }

        }
    }


    private void MovePositions()
    {
        switch (cursorPosition)
        {
            case 0:
                m_this_position.anchoredPosition = new Vector2(-125, -110);
                audioSource.PlayOneShot(CursorSE);
                break;
            case 1:
                m_this_position.anchoredPosition = new Vector2(-125, -145);
                audioSource.PlayOneShot(CursorSE);
                break;
            case 2:
                m_this_position.anchoredPosition = new Vector2(-125, -180);
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
                SceneManager.LoadScene("ChooseCharactor");
                break;
            case 1:
                break;
            case 2:
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

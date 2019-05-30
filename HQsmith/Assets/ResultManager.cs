using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        winnerPreview(winner);
    }

    void winnerPreview(int i)
    {
        switch (i)
        {
            case 0: win.enabled = true;
                break;
            case 1: lose.enabled = true;
                break;
            default:
                break;
        }
    }
}

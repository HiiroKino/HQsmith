using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool Player = false;
    public bool Enemy = false;

    public static int winner = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            winner = 0;
        }

        if (Enemy)
        {
            winner = 1;
        }
    }

    public static int Winner()
    {
        return winner;
    }
}

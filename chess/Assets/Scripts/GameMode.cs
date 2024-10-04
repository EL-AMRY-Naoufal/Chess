using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public void IAMode()
    {
        boardmanager.GameMode = 2;
        SceneManager.LoadScene(1);
    }
    public void Mode1v1()
    {
        boardmanager.GameMode = 1;
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        boardmanager.isWhiteturn = true;
        Score.WhiteScore = 0;
        Score.BlackScore = 0;
        SceneManager.LoadScene(0);
    }

}

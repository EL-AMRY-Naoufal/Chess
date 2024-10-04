using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    public void ReplayGame()
    {
        boardmanager.isWhiteturn = !boardmanager.isWhiteturnR;
        boardmanager.isWhiteturnR = !boardmanager.isWhiteturnR;
        SceneManager.LoadScene(1);
        if (boardmanager.GameMode == 2)
            boardmanager.isWhiteturn = true;
    }

    public void LoadMenu()
    {
        boardmanager.isWhiteturn = true;
        Score.WhiteScore = 0;
        Score.BlackScore = 0;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}

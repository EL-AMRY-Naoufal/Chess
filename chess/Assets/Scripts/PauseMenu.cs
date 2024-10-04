using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject PauseMenuUI;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPause = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        PauseMenuUI.SetActive(true);
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        boardmanager.isWhiteturn = true;
        Score.WhiteScore = 0;
        Score.BlackScore = 0;
        GameIsPause = false;
        SceneManager.LoadScene(0);
    }

    public void QuitMenu()
    {
        Application.Quit();
    }

}

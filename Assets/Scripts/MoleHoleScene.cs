using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoleHoleScene : MonoBehaviour
{
    public List<GameObject> lives;
    public GameObject pausePanel, gameOverPanel;
    public Text score,game_over_text, highscore_text;
    public bool playing = true;
    public bool paused = false;
    private int kill_cout = 0;
    public Status status;

    public void removeLife()
    {
        if (lives.Count > 0)
        {
            Destroy(lives[lives.Count - 1]);
            lives.RemoveAt(lives.Count - 1);
            if(lives.Count <= 0)
            {
                playing = false;
                gameOverPanel.SetActive(true);
                if(status.WackScore < kill_cout)
                {
                    highscore_text.text = "Parabéns você bateu seu record!";
                    highscore_text.fontSize = 30;
                    game_over_text.text = "Racistas mortos\n" + kill_cout.ToString();
                    status.WackScore = kill_cout;
                }
                else
                {
                    highscore_text.text = "Você perdeu";
                    highscore_text.fontSize = 40;
                    game_over_text.text = "Racistas mortos\n" + kill_cout.ToString() + 
                        "\nMaior pontuação\n"+status.WackScore.ToString();
                }
                status.rechargeHappines(kill_cout*5);
            }
        }
    }

    public void unpause()
    {
        paused = false;
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(playing)
            {
                paused = !paused;
                pausePanel.SetActive(!pausePanel.activeSelf);
            }
        }
    }

    public void playAgain()
    {
        SceneManager.LoadScene("WackTHeMole");
    }

    public void returnHome()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void addKill()
    {
        kill_cout++;
        score.text = kill_cout.ToString();
    }
}

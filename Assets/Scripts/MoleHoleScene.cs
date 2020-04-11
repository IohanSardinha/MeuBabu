using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleHoleScene : MonoBehaviour
{
    public List<GameObject> lives;
    public Text text;
    public bool playing = true;
    private int kill_cout = 0;
    public void removeLife()
    {
        if (lives.Count > 0)
        {
            Destroy(lives[lives.Count - 1]);
            lives.RemoveAt(lives.Count - 1);
            if(lives.Count <= 0)
            {
                playing = false;
                Debug.Log("You Lose");
            }
        }
    }

    public void addKill()
    {
        kill_cout++;
        text.text = kill_cout.ToString();
    }
}

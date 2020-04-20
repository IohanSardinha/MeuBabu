using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HangGlidingScene : MonoBehaviour
{
    public static float ACELERATION = 0.05f;
    public GameObject gameOverPanel, pausePanel;
    public List<GameObject> obstacleSprites;
    private List<GameObject> obstacles = new List<GameObject>();
    public GameObject obstaclePrefab;
    public GameObject hangGlider,particles;
    public bool playing = false, started = false, paused = false;
    Vector3 topRight, bottomRight, leftBorder;
    private float speed = 4;
    private float time_between_counter = 0, time_between = 1f;
    private float start_delay = 3f;
    public RepeatingBackground background, background2, background3, background4;
    public Text score_text, message_text, title_text;
    public Status status;
    public float score = 0;

    void Awake()
    {
        bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        leftBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height/2, 0));
    }

    private void AddObstacle()
    {
        GameObject new_obstacle = Instantiate(obstacleSprites[Random.Range(0, obstacleSprites.Count)]);
        Vector3 size = new_obstacle.GetComponent<SpriteRenderer>().bounds.size;
        new_obstacle.transform.position = new Vector3(bottomRight.x + size.x / 2,
                                                Random.Range(bottomRight.y + size.y / 2,
                                                topRight.y - size.y / 2));
        obstacles.Add(new_obstacle);
    }

    private void playingUpdate()
    {
        time_between_counter += Time.deltaTime;
        if (time_between_counter > time_between)
        {
            AddObstacle();
            time_between_counter = 0;
            time_between -= 0.005f; if (time_between <= 0.2) time_between = 0.2f;
        }

        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.Translate(-speed * Time.deltaTime, 0, 0);
            float width = obstacle.GetComponent<SpriteRenderer>().bounds.size.x;
            if (obstacle.transform.position.x + width / 2 < leftBorder.x)
            {
                Destroy(obstacle);
                obstacles.Remove(obstacle);
                break;
            }
        }
        speed += ACELERATION * Time.deltaTime;
        background.speed = speed * 0.25f;
        background2.speed = speed * 0.15f;
        background3.speed = speed * 0.05f;
        background4.speed = speed * 0.01f;
        score += speed * Time.deltaTime;
        score_text.text = Mathf.Ceil(score).ToString() + "m";
        float height = hangGlider.GetComponent<SpriteRenderer>().bounds.size.y;
        if (hangGlider.transform.position.y + (height / 2) * Mathf.Sin(Mathf.PI / 4) <= bottomRight.y)
        {
            playing = false;
            particles.SetActive(false);
            background.speed = 0;
            background2.speed = 0;
            background3.speed = 0;
            background4.speed = 0;
            gameOverPanel.SetActive(true);
            score_text.text = "";

            if(score > status.GlideScore)
            {
                message_text.text = "Parabéns você bateu seu record!";
                message_text.fontSize = 30;
                title_text.text = "Distancia percorrida\n" + Mathf.Ceil(score).ToString() + "m";
                status.GlideScore = (int)Mathf.Ceil(score);
            }
            else
            {
                message_text.text = "Você perdeu";
                message_text.fontSize = 40;
                title_text.text = "Distancia percorrida\n" + Mathf.Ceil(score).ToString() +
                    "m\nMaior distancia\n" + status.GlideScore.ToString()+"m";
            }

            status.rechargeHappines(score/5);
            status.Skin = GameData.Skin.Glider;
        }
    }

    public void removeObstacle(GameObject obstacle)
    {
        obstacles.Remove(obstacle);
    }

    public void unpause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        paused = !paused;
        if (paused) { background.speed = 0; background2.speed = 0; background3.speed = 0; background4.speed = 0; }
    }


    public void returnHome()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void replay()
    {
        SceneManager.LoadScene("HangGliding");
    }

    void Update()
    {
        if(!started)
        {
            start_delay -= Time.deltaTime;
            if(start_delay < 0)
            {
                playing = true;
                started = true;
                particles.SetActive(true);
            }
        }
        else if(playing)
        {
            if(!paused)playingUpdate();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                paused = !paused;
                pausePanel.SetActive(!pausePanel.activeSelf);
                if (paused) { background.speed = 0; background2.speed = 0; background3.speed = 0; background4.speed = 0; }
                }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DanceHeroScene : MonoBehaviour
{

    public List<Color> colors;
    public List<GameObject> spawns;
    public List<CircleButton> buttons;
    public GameObject node;
    private List<GameObject> nodes = new List<GameObject>();
    float temp = 0;
    private float node_speed = 2;
    private float bottom_coord;
    public Animator shine;
    public Animator dancer;
    public Text score_text;
    private float score;
    private float life = 12;
    private float time_till_next = 1;
    private float up_range = 3, bootom_range = 1;
    public GameObject game_over, pausePanel;
    private float start_delay = 2.5f;
    public Text title_text, message_text;
    public Status status;
    public bool paused = false;

    private void Start()
    {
        bottom_coord = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        dancer.speed = 0;
    }

    public void removeNode(GameObject node, bool perfect)
    {
        nodes.Remove(node);
        Destroy(node);
        if (perfect) { dancer.speed += 0.15f; life += 0.4f; }
        else { dancer.speed += 0.075f; life += 0.2f; }
        if (dancer.speed > 5) dancer.speed = 5f;
        if (life > 12) life = 12;
    }

    private void moveNodes()
    {
        foreach(GameObject node in nodes)
        {
            node.transform.Translate(0, -node_speed*Time.deltaTime, 0);
            float height = node.GetComponent<SpriteRenderer>().bounds.size.y;
            if (node.transform.position.y + height / 2 < bottom_coord)
            {
                Destroy(node);
                nodes.Remove(node);
                shine.SetBool("shine", true);
                foreach(CircleButton bttn in buttons)
                {
                    bttn.removeNode(node);
                }
                dancer.speed -= 2.5f; if (dancer.speed < 0) dancer.speed = 0;
                life -= 4f;
                if(life <= 0)
                {
                    score_text.text = "";
                    game_over.SetActive(true);
                    if(score > status.DanceScore)
                    {
                        message_text.text = "Parabéns você bateu seu record!";
                        message_text.fontSize = 30;
                        title_text.text = "Pontos\n" + Mathf.Ceil(score).ToString();
                        status.DanceScore = (int)Mathf.Ceil(score);
                    }
                    else
                    {
                        message_text.text = "Você perdeu";
                        message_text.fontSize = 40;
                        title_text.text = "Pontos\n" + Mathf.Ceil(score).ToString() +
                            "\nMaior pontuação\n" + status.DanceScore.ToString();
                    }
                    status.rechargeHappines(score / 50);
                }
                break;
            }
        }
    }

    public void returnHome()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void replay()
    {
        SceneManager.LoadScene("DanceHero");
    }

    public void unpause()
    {
        pausePanel.SetActive(false);
        paused = false;
        dancer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(start_delay > 0)
        {
            start_delay -= Time.deltaTime;
            return;
        }
        if (life > 0 && !paused)
        {
            temp += Time.deltaTime;
            if (temp > time_till_next)
            {
                temp = 0;
                int index = Random.Range(0, spawns.Count);
                GameObject new_node = Instantiate(node, spawns[index].transform.position, new Quaternion());
                new_node.GetComponent<SpriteRenderer>().color = colors[index];
                nodes.Add(new_node);
                buttons[index].nodes.Add(new_node);
                time_till_next = Random.Range(bootom_range, up_range);
                bootom_range -= 0.12f; if (bootom_range < 0.3) bootom_range = 0.3f;
                up_range -= 0.06f; if (up_range < 0.5) up_range = 0.5f;
            }
            if (dancer.speed > 0) dancer.speed -= 0.01f * Time.deltaTime;
            score += dancer.speed * 20 * Time.deltaTime;
            score_text.text = Mathf.Ceil(score).ToString();
            node_speed += 0.12f * Time.deltaTime; if (node_speed > 7) node_speed = 7;
            moveNodes();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            pausePanel.SetActive(!pausePanel.activeSelf);
            dancer.enabled = dancer.enabled;
        }
    }
}

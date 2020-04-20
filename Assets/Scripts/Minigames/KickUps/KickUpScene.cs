using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KickUpScene : MonoBehaviour
{

    private static readonly float GRAVITY = -9.8f, BOUNCE = 10, MAX_X_VEL = 5, FORCE_MULTIPLYER = 12;
    private Vector3 velocity = new Vector3(1, BOUNCE);
    private float left_coords, right_coords, middle_coords;
    float width, height;
    float babu_width, babu_height;
    public GameObject babu, head;
    public float start_delay = 3f;
    private int score = 0;
    public Text score_text, message_text, title_text;
    public bool playing = true, paused = false, started = false;
    public Status status;
    public GameObject game_over, pausePanel;
    public AudioSource ballhit;
    private Animator animator;

    void Start()
    {
        left_coords = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        right_coords = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        middle_coords = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, 0, 0)).x;
        Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
        width = size.x;
        height = size.y;

        size = babu.GetComponent<SpriteRenderer>().bounds.size;
        babu_width = size.x;
        babu_height = size.y;

        animator = babu.GetComponent<Animator>();
    }

    void Update()
    {
        if (start_delay > 0)
        {
            start_delay -= Time.deltaTime;
            return;
        }
        if(!started)
        {
            animator.SetBool("Started", true);
            started = true;
        }
        if (playing && !paused)
        {
            velocity.y += GRAVITY * Time.deltaTime;
            if (transform.position.x + width / 2 > right_coords || transform.position.x - width / 2 < left_coords)
            {
                velocity.x *= -1;
                transform.position += new Vector3(velocity.x, 0) * Time.deltaTime;
            }
            transform.position += velocity * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, velocity.x));
            if (transform.position.y + height / 2 > babu.transform.position.y - babu_height / 2)
            {
                if (transform.position.x > middle_coords)
                {
                    babu.transform.position = new Vector3(transform.position.x -0.4f, babu.transform.position.y);
                    babu.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    Vector3 diff = transform.position - head.transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    head.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
                    if (Mathf.Atan2(diff.y, diff.x) > 0.5) head.transform.rotation = Quaternion.Euler(0f, 0f, 0.5f * Mathf.Rad2Deg);
                    if (Mathf.Atan2(diff.y, diff.x) < -0.5) head.transform.rotation = Quaternion.Euler(0f, 0f, -0.5f * Mathf.Rad2Deg);
                }
                else
                {
                    babu.transform.position = new Vector3(transform.position.x + 0.4f, babu.transform.position.y);
                    babu.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                    Vector3 diff = head.transform.position - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    head.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
                    Debug.Log(Mathf.Atan2(diff.y, diff.x));
                    if (Mathf.Atan2(diff.y, diff.x) < -0.5) head.transform.rotation = Quaternion.Euler(0f, 0f, -0.5f * Mathf.Rad2Deg);
                    if (Mathf.Atan2(diff.y, diff.x) > 0.5) head.transform.rotation = Quaternion.Euler(0f, 0f, 0.5f * Mathf.Rad2Deg);
                }
            }
            else
            {
                playing = false;
                game_over.SetActive(true);
                score_text.text = "";

                if (score > status.KickScore)
                {
                    message_text.text = "Parabéns você bateu seu record!";
                    message_text.fontSize = 30;
                    title_text.text = "Embaixadinhas\n" + score.ToString();
                    status.KickScore = score;
                }
                else
                {
                    message_text.text = "Você perdeu";
                    message_text.fontSize = 40;
                    title_text.text = "Embaixadinhas\n" + score.ToString() +
                        "\nRecord\n" + status.KickScore.ToString();
                }

                status.rechargeHappines(score * 5);
                status.Skin = GameData.Skin.Football;
            }
        }
        if(playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = !paused;
                pausePanel.SetActive(!pausePanel.activeSelf);
            }
        }
    }

    public void unpause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        paused = !paused;
    }

    public void returnHome()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void replay()
    {
        SceneManager.LoadScene("KickUps");
    }

    private void OnMouseOver()
    {
        if (start_delay > 0) return;
        if (Input.GetMouseButtonDown(0) && transform.position.y < babu.transform.position.y + babu_height/3 && transform.position.y > babu.transform.position.y - babu_height / 2)
        {
            ballhit.Play();
            velocity.y = BOUNCE;
            Vector3 distance_vector = new Vector3(transform.position.x, transform.position.y + height / 2) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float force = Mathf.Sin(Mathf.Atan(distance_vector.x / distance_vector.y)) * FORCE_MULTIPLYER;
            if (Mathf.Abs(velocity.x) < 0.2) velocity.x = -5f;
            velocity.x = Mathf.Abs(velocity.x) * force;
            if (velocity.x > MAX_X_VEL) velocity.x = MAX_X_VEL;
            if (velocity.x < -MAX_X_VEL) velocity.x = -MAX_X_VEL;
            score++;
            score_text.text = score.ToString();
            animator.SetBool("Kick", true);
        }
    }
}

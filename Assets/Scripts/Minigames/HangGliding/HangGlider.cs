using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangGlider : MonoBehaviour
{

    private static float GRAVITY = -9;
    private static float UP_SPEED = 9;
    private float velocity = 0;
    public HangGlidingScene gameMaster;
    float topCoordinate;
    private bool gliding = true;
    public GameObject explosion;
    public AudioSource crash;

    private void Awake()
    {
        topCoordinate = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameMaster.playing || gameMaster.paused) return;
        if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) && gliding)
        {
            velocity = 5;
        }
        /*else if (Input.GetMouseButton(0) && gliding)
        {
            velocity += UP_SPEED * Time.deltaTime;
            if (transform.rotation.eulerAngles.z < 270+45) transform.Rotate(0, 0, 1);
        }*/
        else
        {
            velocity += GRAVITY*Time.deltaTime;
            if (velocity < 0 && transform.rotation.eulerAngles.z > 270-45) transform.Rotate(0, 0, -0.5f);
            if (velocity > 0 && transform.rotation.eulerAngles.z < 270 + 45) transform.Rotate(0, 0, 1);
        }
        Vector3 old_pos = transform.position;
        Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
        transform.position += new Vector3(0, velocity * Time.deltaTime);
        if (transform.position.y + (size.y / 2) * Mathf.Sin(Mathf.PI / 4) >= topCoordinate) transform.position = old_pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gliding)
        {
            gameMaster.removeObstacle(collision.gameObject);
            gliding = false;
            velocity = 0;
            Instantiate(explosion, collision.gameObject.transform.position, new Quaternion());
            Destroy(collision.gameObject);
            crash.Play();
        }
    }
}

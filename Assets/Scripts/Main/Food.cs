using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    private float startPosX;
    private float startPosY;
    private bool held;
    private float freed_time = 0;
    private StatusManager statusM;
    private Status status;
    public List<Sprite> foods;
    public GameObject desapearFreed, desapearEaten;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = foods[Random.Range(0, foods.Count)];
    }

    public void setStatus(StatusManager statusM, Status status)
    {
        this.statusM = statusM;
        this.status = status;
    }


    // Update is called once per frame
    void Update()
    {
        if (status.Sleeping || statusM.choosing_game)
        {
            statusM.Food_count--;
            Instantiate(desapearFreed, transform.position, transform.rotation);
            Destroy(this.gameObject);
            statusM.desapear.Play();
        }
        if (held)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y+0.7f, 0);
            freed_time = 0;
        }
        else
        {
            freed_time += Time.deltaTime;
            if (freed_time > 3)
            {
                statusM.Food_count--;
                Instantiate(desapearFreed, transform.position, transform.rotation);
                Destroy(this.gameObject);
                statusM.desapear.Play();
            }
        }
    }


    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            held = true;
            statusM.disableWardrobe();
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rosto"))
        {
            if (!statusM.eating.isPlaying) statusM.eating.Play();
            transform.localScale -= Vector3.one * Time.deltaTime * 0.25f;
            if (transform.localScale.x < 0.1)
            {
                statusM.reduceHunger(25);
                statusM.Food_count--;
                Instantiate(desapearEaten, transform.position, transform.rotation);
                Destroy(gameObject);
                statusM.sparkle.Play();
            }
        }
    }


    private void OnMouseUp()
    {
        held = false;
    }
}

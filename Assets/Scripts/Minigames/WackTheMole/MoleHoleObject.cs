using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MoleHoleObject : MonoBehaviour
{

    public MoleHoleScene gameMaster;
    public bool open = false;
    private float open_time = 2f;
    private float closed_time;
    private float open_time_count = 0;
    private float closed_time_count = 0;
    private float closed_range_init = 3f, closed_range_end = 5f;
    private float racist_proportion = 0.5f;
    private Animator animator;
    public GameObject target;
    public GameObject fire;
    public List<Sprite> racists;
    public List<Sprite> people;
    public bool racist;
    public AudioSource racistScream, Scream, Fire;

    private void Start()
    {
        animator = GetComponent<Animator>();
        closed_time = Random.Range(closed_range_init, closed_range_end);
        setTarget();
    }

    private void setTarget()
    {
        if (Random.Range(0f, 1f) < racist_proportion)
        {
            racist = true;
            target.GetComponent<SpriteRenderer>().sprite = racists[Random.Range(0, racists.Count)];
        }
        else
        {
            racist = false;
            target.GetComponent<SpriteRenderer>().sprite = people[Random.Range(0, people.Count)];
        }
    }

    private void setOpen(bool o)
    {
        //open = o;
        animator.SetBool("open", o);
    }

    void Update()
    {
        if (gameMaster.paused) {}
        else if (!gameMaster.playing)
        {
            setOpen(false);
        }
        else if (open)
        {
            open_time_count += Time.deltaTime;
            if (open_time_count >= open_time)//Close
            {
                setOpen(false);
                open_time_count = 0;
                //if (racist) gameMaster.removeLife();
            }
        }
        else
        {
            closed_time_count += Time.deltaTime;
            if (closed_time_count >= closed_time)//Open
            {
                setOpen(true);
                closed_time_count = 0;
                setTarget();
                closed_range_init -= 0.12f; if (closed_range_init < 0.8f) closed_range_init = 0.8f;
                closed_range_end -= 0.15f; if (closed_range_end < 1f) closed_range_end = 1f;
                open_time -= 0.2f; if (open_time < 1f) open_time = 1f;
                closed_time = Random.Range(closed_range_init, closed_range_end);
            }
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && open)
        {
            Instantiate(fire, transform);
            setOpen(false);
            open_time_count = 0;
            if(!racist)
            {
                gameMaster.removeLife();
                Fire.Play();
                Scream.Play();
            }
            if (racist)
            {
                racist = false;
                gameMaster.addKill();
                Fire.Play();
                racistScream.Play();
            }
        }
    }
}

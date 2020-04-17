using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleButton : MonoBehaviour
{
    public DanceHeroScene gameMaster;
    public List<GameObject> nodes = new List<GameObject>();
    public GameObject stars, dust;
    public List<AudioSource> notes;
    public AudioSource wrong, almost;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void removeNode(GameObject node)
    {
        if(nodes.Contains(node))
        {
            nodes.Remove(node);
        }
    }

    private void OnMouseOver()
    {
        if (gameMaster.paused) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (nodes.Count <= 0)
            {
                animator.SetInteger("ID", 2);
                wrong.Play();
                return;
            }

            float distance = Mathf.Abs(nodes[0].transform.position.y - transform.position.y);
            if (distance > 0.6)
            {
                animator.SetInteger("ID", 2);
                wrong.Play();
            }
            else if (distance > 0.2)
            {
                Instantiate(dust, nodes[0].transform.position, new Quaternion());
                animator.SetInteger("ID", 3);
                almost.Play();
                gameMaster.removeNode(nodes[0],false);
                nodes.RemoveAt(0);
            }
            else
            {
                Instantiate(stars, nodes[0].transform.position, new Quaternion());
                animator.SetInteger("ID", 1);
                notes[Random.Range(0,notes.Count)].Play();
                gameMaster.removeNode(nodes[0], true);
                nodes.RemoveAt(0);
            }
        }
    }
}

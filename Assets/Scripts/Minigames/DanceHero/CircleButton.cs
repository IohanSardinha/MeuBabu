using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleButton : MonoBehaviour
{
    public DanceHeroScene gameMaster;
    public List<GameObject> nodes = new List<GameObject>();
    public GameObject stars, dust;

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
                return;
            }

            float distance = Mathf.Abs(nodes[0].transform.position.y - transform.position.y);
            if (distance > 1.2)
            {
                animator.SetInteger("ID", 2);
            }
            else if (distance > 0.5)
            {
                Instantiate(dust, nodes[0].transform.position, new Quaternion());
                animator.SetInteger("ID", 3);
                gameMaster.removeNode(nodes[0],false);
                nodes.RemoveAt(0);
            }
            else
            {
                Instantiate(stars, nodes[0].transform.position, new Quaternion());
                animator.SetInteger("ID", 1);
                gameMaster.removeNode(nodes[0], true);
                nodes.RemoveAt(0);
            }
        }
    }
}

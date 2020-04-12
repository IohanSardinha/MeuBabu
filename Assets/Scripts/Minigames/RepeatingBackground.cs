using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    public float speed;
    public GameObject first, second;
    float width, leftBorder;
    void Start()
    {
        width = first.GetComponent<SpriteRenderer>().bounds.size.x;
        leftBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0)).x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);
        if(first.transform.position.x + width/2 < leftBorder)
        {
            first.transform.position = new Vector3(second.transform.position.x + width,second.transform.position.y,second.transform.position.z);
            GameObject temp = first;
            first = second;
            second = temp;
        }
    }
}

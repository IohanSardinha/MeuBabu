using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choveiro : MonoBehaviour
{

    private float startPosX;
    private float startPosY;
    private bool held;
    private float freed_time = 0;
    private Status status;
    private GameObject spawn;
    public GameObject desapear;

    public void setStatus(Status status)
    {
        this.status = status;
    }

    public void setSpawn(GameObject spawn)
    {
        this.spawn = spawn;
    }

    // Update is called once per frame
    void Update()
    {
        if(held)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
            freed_time = 0;
        }
        else
        {
            freed_time += Time.deltaTime;
            if(freed_time > 1.5)
            {
                spawn.transform.position = this.transform.position;
                status.Showering = false;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Instantiate(desapear, transform.position, transform.rotation);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            held = true;
            status.disableWardrobe();
        }
    }

    private void OnMouseUp()
    {
        held = false;
    }

}

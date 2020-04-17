using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choveiro : MonoBehaviour
{

    private float startPosX;
    private float startPosY;
    private bool held;
    private float freed_time = 0;
    private StatusManager statusM;
    private Status status;
    private GameObject spawn;
    public GameObject desapear;

    private void Start()
    {
        if (!statusM.showerAudio.isPlaying) statusM.showerAudio.Play();
    }

    public void setStatus(StatusManager statusM, Status status)
    {
        this.statusM = statusM;
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
                statusM.Showering = false;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Instantiate(desapear, transform.position, transform.rotation);
        statusM.showerAudio.Stop();
        statusM.desapear.Play();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            held = true;
            statusM.disableWardrobe();
        }
    }

    private void OnMouseUp()
    {
        held = false;
    }

}

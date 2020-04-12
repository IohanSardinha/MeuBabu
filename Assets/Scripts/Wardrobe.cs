using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wardrobe : MonoBehaviour
{
    public List<Sprite> images;
    public List<GameData.Skin> skins;
    public Status status;
    public GameObject babu;
    public Image nextSkin, previousSkin;
    private int curr;
    private float inactive_time = 0;

    private void Awake()
    {
        curr = skins.IndexOf(status.Skin);
        changeSkin(0);
    }

    public void changeSkin(int n)
    {
        inactive_time = 0;
        curr = (curr + n) % skins.Count;
        if (curr < 0) curr = skins.Count + curr;
        status.Skin = skins[curr];
        int nex = (curr + 1) % skins.Count;
        if (nex < 0) nex = skins.Count + nex;
        nextSkin.sprite = images[nex];
        int prev = (curr - 1) % skins.Count;
        if (prev < 0) prev = skins.Count + prev;
        previousSkin.sprite = images[prev];
        babu.GetComponent<Babu>().changeSkin();
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            inactive_time += Time.deltaTime;
            if(inactive_time > 5)
            {
                inactive_time = 0;
                gameObject.SetActive(false);
            }
        }
        else
        {
            inactive_time = 0;
        }
    }

}

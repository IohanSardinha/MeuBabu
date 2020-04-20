using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Babu : MonoBehaviour
{
    private static float CLEANING_FACTOR = 10f;

    public StatusManager statusM;
    public Status status;
    private Animator animator;
    public Wardrobe wardrobe;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnParticleCollision(GameObject other)
    {
        statusM.rechargeHygiene(Time.deltaTime * CLEANING_FACTOR);
    }

    public void changeSkin()
    {
        GetComponent<SpriteRenderer>().sprite = wardrobe.images[wardrobe.skins.IndexOf(status.Skin)];
        animator.SetInteger("Skin", wardrobe.skins.IndexOf(status.Skin));
    }
}

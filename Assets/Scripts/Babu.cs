using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Babu : MonoBehaviour
{
    private static float CLEANING_FACTOR = 10f;

    public Status status;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnParticleCollision(GameObject other)
    {
        status.rechargeHygiene(Time.deltaTime * CLEANING_FACTOR);
    }

    public void changeSkin()
    {
        List<GameData.Skin> SKIN_ID_ORDER = new List<GameData.Skin>(){
            GameData.Skin.Underwear,
            GameData.Skin.Shirt_and_shorts,
            GameData.Skin.Bunny
        };
        animator.SetInteger("Skin",SKIN_ID_ORDER.IndexOf(status.Skin));
    }
}

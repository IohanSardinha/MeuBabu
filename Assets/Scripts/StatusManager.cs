using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    private static float TIME_FACTOR = 0.1f;
    private static float SLEEPING_FACTOR = 2f;

    public Image energyImage,hygieneImage,hungerImage,happinessImage;
    public Status status;
    public Sprite happySprite, mediumHapinessSprite, almostSadSprite, sadSprite;

    private void Update()
    {
        energyImage.fillAmount = status.Energy/100;
        hygieneImage.fillAmount = 1 - status.Hygiene / 100;
        hungerImage.fillAmount = 1 - status.Hunger / 100;
        if(status.Happiness >= 75)
        {
            happinessImage.sprite = happySprite;
        }
        else if(status.Happiness >= 50)
        {
            happinessImage.sprite = mediumHapinessSprite;
        }
        else if(status.Happiness >= 25)
        {
            happinessImage.sprite = almostSadSprite;
        }
        else
        {
            happinessImage.sprite = sadSprite;
        }

        float factor;

        if (status.Sleeping)
        {
            factor = Time.deltaTime * TIME_FACTOR/2;
            status.Energy += Time.deltaTime * SLEEPING_FACTOR; if (status.Energy > 100) status.Energy = 100;
            status.Hunger += factor; if (status.Hunger > 100) status.Hunger = 100;
            status.Happiness -= factor; if (status.Happiness < 0) status.Happiness = 0;
            status.Hygiene -= factor; if (status.Hygiene < 0) status.Hygiene = 0;
        }
        else
        {
            factor = Time.deltaTime* TIME_FACTOR;
            status.Energy -= factor; if (status.Energy < 0) status.Energy = 0;
            status.Hunger += factor; if (status.Hunger > 100) status.Hunger = 100;
            status.Happiness -= factor; if (status.Happiness < 0) status.Happiness = 0;
            status.Hygiene -= factor; if (status.Hygiene < 0) status.Hygiene = 0;
        }
        
    }



}

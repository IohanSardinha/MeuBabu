using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    private static float TIME_FACTOR = 0.1f;
    private static float SLEEPING_FACTOR = 5f;

    private bool showering = false;
    private bool changing_clothes = false;
    public bool choosing_game = false;
    private int food_count = 0;
    private float speechBoubleTime = 0;
    private bool speechBoubleShowing = false;

    public Image energyImage,hygieneImage,hungerImage,happinessImage;
    public Status status;
    public Sprite happySprite, mediumHapinessSprite, almostSadSprite, sadSprite;
    public GameObject lights;
    public GameObject babu;
    public GameObject showerPrefab, showerSpawn;
    public GameObject foodPrefab, foodSpawn;
    public GameObject wardrobe;
    public GameObject games;
    public GameObject speechBouble;
    public Text speechBoubleText;
    private GameObject shower;

    public int Food_count { get => food_count; set => food_count = value; }
    public bool Showering { get => showering; set => showering = value; }

    private void Start()
    {
        lights.SetActive(status.Sleeping);
        babu.GetComponent<Babu>().changeSkin();
        
        if (status.Sleeping)
        {
            babu.transform.Rotate(0, 0, 80);
        }
        babu.GetComponent<Animator>().enabled = !status.Sleeping;
    }

    public void startScene(string scene)
    {

        SceneManager.LoadScene(scene);
    }

    private void showScpeechBouble(string text)
    {
        if (status.Sleeping) return;
        speechBoubleText.text = text;
        speechBouble.SetActive(true);
        speechBoubleShowing = true;
    }

    public void chooseGame()
    {
        if (status.Energy <= 0)
        {
            showScpeechBouble("Tô muito cansado pra brincar agora");
            return;
        }
        if (status.Sleeping) return;
        disableWardrobe();
        games.SetActive(true);
        choosing_game = true;
        if (showering)
        {
            if (shower != null)
            {
                Destroy(shower);
                showerSpawn.transform.position = shower.transform.position;
            }
            showering = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(choosing_game)
            {
                games.SetActive(false);
                choosing_game = false;
            }
        }

        if(speechBoubleShowing)
        {
            speechBoubleTime += Time.deltaTime;
            if(speechBoubleTime >= 5)
            {
                speechBoubleTime = 0;
                speechBoubleShowing = false;
                speechBouble.SetActive(false);
            }
        }

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

            if (status.Energy < 10 && !speechBoubleShowing) showScpeechBouble("Que sono cara...");
            else if (status.Hunger > 90 && !speechBoubleShowing) showScpeechBouble("Mó fome");
            else if (status.Happiness < 10 && !speechBoubleShowing) showScpeechBouble("Vamos brincar?!");
            else if(status.Hygiene < 10 && !speechBoubleShowing) showScpeechBouble("Tô muito sujo mermão");
        }
        

    }

    public void toggleWardrobe()
    {
        if (status.Sleeping) return;
        changing_clothes = !changing_clothes;
        wardrobe.SetActive(!wardrobe.activeSelf);
    }

    public void disableWardrobe()
    {
        if (changing_clothes) { wardrobe.SetActive(false); changing_clothes = false; }
    }

    public void toggleSleep()
    {
        if(speechBoubleShowing)
        {
            speechBoubleShowing = false;
            speechBoubleTime = 0;
            speechBouble.SetActive(false);
        }
        status.Sleeping = !status.Sleeping;
        lights.SetActive(!lights.activeSelf);
        babu.GetComponent<Animator>().enabled = !babu.GetComponent<Animator>().enabled;
        disableWardrobe();
        if (status.Sleeping)
        {
            if (showering)
            {
                if (shower != null)
                {
                    Destroy(shower);
                    showerSpawn.transform.position = shower.transform.position;
                }
                showering = false;
            }
            babu.transform.Rotate(0, 0, 80);
        }
        else
        {
            babu.transform.Rotate(0, 0, -80);
        }

    }

    public void giveFood()
    {
        disableWardrobe();
        if (Food_count >= 3 || status.Sleeping) return;
        Instantiate(foodPrefab, foodSpawn.transform.position, foodSpawn.transform.rotation).GetComponent<Food>().setStatus(this,status);
        Food_count++;
    }

    public void startShower()
    {
        disableWardrobe();
        if (status.Sleeping) return;
        if (showering)
        {
            if (shower != null)
            {
                Destroy(shower);
                showerSpawn.transform.position = shower.transform.position;
            }
            showering = false;
            return;
        }
        showering = true;
        shower = Instantiate(showerPrefab, showerSpawn.transform.position, showerSpawn.transform.rotation);
        shower.GetComponent<Choveiro>().setStatus(this, status);
        shower.GetComponent<Choveiro>().setSpawn(showerSpawn);
    }

    public void rechargeHygiene(float amout)
    {
        if (status.Sleeping) return;
        status.Hygiene += amout;
        if (status.Hygiene > 100) status.Hygiene = 100;
    }

    public void rechargeEnergy(float amout)
    {
        if (status.Sleeping) return;
        status.Energy += amout;
        if (status.Energy > 100) status.Energy = 100;
    }


    public void reduceHunger(float amout)
    {
        if (status.Sleeping) return;
        status.Hunger -= amout;
        if (status.Hunger < 0) status.Hunger = 0;
    }

}

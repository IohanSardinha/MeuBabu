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
    private bool closing = false;

    public Image energyImage,hygieneImage,hungerImage,happinessImage;
    public Status status;
    public Sprite happySprite, mediumHapinessSprite, almostSadSprite, sadSprite;
    public GameObject lights;
    public GameObject babu;
    public GameObject showerPrefab, showerSpawn;
    public GameObject foodPrefab, foodSpawn;
    public GameObject wardrobe;
    public GameObject games, closingPanel;
    public GameObject speechBouble;
    public Text speechBoubleText;
    private GameObject shower;
    public AudioSource music, snoring;
    public AudioSource eating, sparkle, desapear, showerAudio;
    private GameData.Skin oldSkin = GameData.Skin.Pijama;

    public int Food_count { get => food_count; set => food_count = value; }
    public bool Showering { get => showering; set => showering = value; }

    private void Start()
    {
        lights.SetActive(status.Sleeping);
        babu.GetComponent<Babu>().changeSkin();
        
        if (status.Sleeping)
        {
            babu.transform.Rotate(0, 0, 80);
            music.Pause();
            snoring.Play();
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

    public void closeGames()
    {
        games.SetActive(false);
        choosing_game = false;
    }

    public void closeClosingPanel()
    {
        closing = false;
        closingPanel.SetActive(false);
    }

    public void closeApplication()
    {
        status.SaveFile();
        Application.Quit();
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
            else
            {
                closing = !closing;
                closingPanel.SetActive(!closingPanel.activeSelf);
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


        status.SaveFile();

    }

    public void disableWardrobe()
    {
        if (changing_clothes) { wardrobe.SetActive(false); changing_clothes = false; }

        status.SaveFile();

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
            oldSkin = status.Skin;
            status.Skin = GameData.Skin.Pijama;
            music.Pause();
            snoring.Play();
        }
        else
        {
            music.UnPause();
            snoring.Stop();
            babu.transform.Rotate(0, 0, -80);
            status.Skin = oldSkin;
        }
        babu.GetComponent<Babu>().changeSkin();
        babu.GetComponent<Animator>().enabled = !babu.GetComponent<Animator>().enabled;
        status.SaveFile();

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

        status.SaveFile();

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

        status.SaveFile();

    }

}

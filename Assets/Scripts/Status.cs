using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Status : MonoBehaviour
{
    public GameObject lights;
    public GameObject babu;
    public GameObject showerPrefab, showerSpawn;
    public GameObject foodPrefab, foodSpawn;
    public GameObject wardrobe;
    private GameObject shower;

    static float TIME_OUT_FACTOR = 0.001f;
    static float SLEEPING_FACTOR = 0.05f;

    private bool sleeping;
    private bool showering = false;
    private bool changing_clothes = false;
    private int food_count = 0;
    private float hunger;
    private float energy;
    private float happiness;
    private float hygiene;
    private GameData.Skin skin;
    public float Hunger { get => hunger; set => hunger = value; }
    public float Energy { get => energy; set => energy = value; }
    public float Happiness { get => happiness; set => happiness = value; }
    public float Hygiene { get => hygiene; set => hygiene = value; }
    public GameData.Skin Skin { get => skin; set => skin = value; }
    public bool Sleeping { get => sleeping; set => sleeping = value; }
    public bool Showering { get => showering; set => showering = value; }
    public int Food_count { get => food_count; set => food_count = value; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        LoadFile();
    }

    private void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        GameData data = new GameData(hunger, energy, sleeping, happiness, hygiene, skin, DateTime.Now);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            hunger = 0;
            energy = 100;
            happiness = 100;
            hygiene = 100;
            sleeping = false;
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        hygiene = data.hygiene;
        hunger = data.hunger;
        energy = data.energy;
        happiness = data.happiness;
        sleeping = data.sleeping;
        lights.SetActive(sleeping);
        skin = data.skin;
        babu.GetComponent<Babu>().changeSkin();
        babu.GetComponent<Animator>().enabled = !sleeping;
        if (sleeping)
        {
            babu.transform.Rotate(0, 0, 80);
        }
        double elepsedTime = (DateTime.Now - data.close_time).TotalSeconds;

        if (elepsedTime < 0)
        {
            hunger = 100;
            energy = 0;
            happiness = 0;
            hygiene = 0;
            sleeping = false;
            return;
        }

        computeTimeOut(elepsedTime);
    }

    private void computeTimeOut(double elepsedTime)
    {
        float factor;
        if (sleeping)
        {
            factor = Convert.ToSingle(elepsedTime * SLEEPING_FACTOR);
            energy += factor;
        }
        else
        {
            factor = Convert.ToSingle(elepsedTime * TIME_OUT_FACTOR);
            energy -= factor;
        }
        hunger += factor;
        happiness -= factor;
        hygiene -= factor;
    }

    public void toggleWardrobe()
    {
        changing_clothes = !changing_clothes;
        wardrobe.SetActive(!wardrobe.activeSelf);
    }

    public void disableWardrobe()
    {
        if (changing_clothes){ wardrobe.SetActive(false); changing_clothes = false; }
    }

    public void toggleSleep()
    {
        sleeping = !sleeping;
        lights.SetActive(!lights.activeSelf);
        babu.GetComponent<Animator>().enabled = !babu.GetComponent<Animator>().enabled;
        disableWardrobe();
        if (sleeping)
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
        if (food_count >= 3 || sleeping) return;
        Instantiate(foodPrefab, foodSpawn.transform.position, foodSpawn.transform.rotation).GetComponent<Food>().setStatus(this.GetComponent<Status>());
        food_count++;
    }
    
    public void startShower()
    {
        disableWardrobe();
        if (sleeping) return;
        if (showering)
        {
            if(shower != null)
            {
                Destroy(shower);
                showerSpawn.transform.position = shower.transform.position;
            }
            showering = false;
            return;
        }
        showering = true;
        shower = Instantiate(showerPrefab, showerSpawn.transform.position,showerSpawn.transform.rotation);
        shower.GetComponent<Choveiro>().setStatus(this.GetComponent<Status>());
        shower.GetComponent<Choveiro>().setSpawn(showerSpawn);
    }

    public void rechargeHygiene(float amout)
    {
        if (sleeping) return;
        hygiene += amout;
        if (hygiene > 100) hygiene = 100;
    }

    public void rechargeEnergy(float amout)
    {
        if (sleeping) return;
        energy += amout;
        if (energy > 100) energy = 100;
    }

    public void rechargeHappines(float amout)
    {
        if (sleeping) return;
        happiness += amout;
        energy -= amout / 2;
        hygiene -= amout / 2;
        hunger += amout / 2;
        if (happiness > 100) happiness = 100;
    }

    public void reduceHunger(float amout)
    {
        if (sleeping) return;
        hunger -= amout;
        if (hunger < 0) hunger = 0;
    }


    void OnApplicationQuit()
    {
        SaveFile();
    }

}


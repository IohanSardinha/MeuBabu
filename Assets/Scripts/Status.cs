using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Status : MonoBehaviour
{
    static float TIME_OUT_FACTOR = 0.001f;
    static float SLEEPING_FACTOR = 0.05f;

    private GameData gameData;

    public float Hunger { get => gameData.hunger; set => gameData.hunger = value; }
    public float Energy { get => gameData.energy; set => gameData.energy = value; }
    public float Happiness { get => gameData.happiness; set => gameData.happiness = value; }
    public float Hygiene { get => gameData.hygiene; set => gameData.hygiene = value; }
    public GameData.Skin Skin { get => gameData.skin; set => gameData.skin = value; }
    public bool Sleeping { get => gameData.sleeping; set => gameData.sleeping = value; }
    public int WackScore { get => gameData.wack_score; set => gameData.wack_score = value; }
    public int GlideScore { get => gameData.glide_score; set => gameData.glide_score= value; }
    public int DanceScore { get => gameData.dance_score; set => gameData.dance_score= value; }
    public int KickScore { get => gameData.kick_score; set => gameData.kick_score = value; }

    public bool SFX { get => gameData.sfx; set => gameData.sfx = value; }
    public bool Music { get => gameData.music; set => gameData.music = value; }
    public float SFXVolume { get => gameData.sfx_volume; set => gameData.sfx_volume = value; }
    public float MusicVolume { get => gameData.music_volume; set => gameData.music_volume = value; }
    private void Awake()
    {
        gameData = new GameData();
        LoadFile();
    }

    private void OnDestroy()
    {
        SaveFile();
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        gameData.close_time = DateTime.Now;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, gameData);
        file.Close();
    }


    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            gameData = new GameData();
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        gameData = data;
        double elepsedTime = (DateTime.Now - data.close_time).TotalSeconds;

        if (elepsedTime < 0)
        {
            gameData = new GameData();
            return;
        }
        computeTimeOut(elepsedTime);
    }

    private void computeTimeOut(double elepsedTime)
    {
        float factor;
        if (Sleeping)
        {
            factor = Convert.ToSingle(elepsedTime * SLEEPING_FACTOR);
            Energy += factor;
        }
        else
        {
            factor = Convert.ToSingle(elepsedTime * TIME_OUT_FACTOR);
            Energy -= factor;
        }
        Hunger += factor;
        Happiness -= factor;
        Hygiene -= factor;
    }

    public void rechargeHappines(float amout)
    {
        Happiness += amout; if (Happiness > 100) Happiness = 100;
        Energy -= amout / 2; if (Energy < 0) Energy = 0;
        Hygiene -= amout / 2; if (Hygiene < 0) Hygiene = 0;
        Hunger += amout / 2; if (Hunger > 100) Hunger = 100;
    }

    private void OnApplicationPause(bool pause)
    {
        SaveFile();
    }

}


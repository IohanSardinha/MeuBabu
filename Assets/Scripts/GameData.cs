﻿using System;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public enum Skin
    {
        Underwear,
        Shirt_and_shorts,
        Bunny,
        Glider,
        Towell
    };

    public float hunger;
    public float energy;
    public bool sleeping;
    public float happiness;
    public float hygiene;
    public Skin skin;
    public DateTime close_time;
    public int wack_score;
    public int glide_score;
    public int dance_score;
    public int kick_score;
    public bool sfx;
    public bool music;
    public float sfx_volume;
    public float music_volume;

    public GameData()
    {
        hunger = 0;
        energy = 100;
        sleeping = false;
        happiness = 100;
        hygiene = 100;
        skin = Skin.Underwear;
        close_time = DateTime.Now;
        wack_score = 0;
        glide_score = 0;
        dance_score = 0;
        kick_score = 0;
        sfx = true;
        music = true;
        sfx_volume = 1;
        music_volume = 0.5f;
    }
}


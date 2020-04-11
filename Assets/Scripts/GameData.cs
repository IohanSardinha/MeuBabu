using System;

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

    public GameData(float hunger, float energy,bool sleeping, float happiness, float hygiene, Skin skin, DateTime close_time)
    {
        this.hunger = hunger;
        this.energy = energy;
        this.sleeping = sleeping;
        this.happiness = happiness;
        this.hygiene = hygiene;
        this.skin = skin;
        this.close_time = close_time;
    }
}


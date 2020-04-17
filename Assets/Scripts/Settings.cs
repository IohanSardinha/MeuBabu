using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public Status status;
    public GameObject settings;
    public List<AudioSource> musics;
    public List<AudioSource> sfxs;
    public Toggle sfx_T, music_T;
    public Slider sfx_S, music_S;
    public bool Main = false;

    void Start()
    {
        if (Main)
        {
            sfx_T.isOn = status.SFX;
            music_T.isOn = status.Music;
            sfx_S.value = status.SFXVolume;
            music_S.value = status.MusicVolume;
        }
        foreach (AudioSource music in musics)
        {
            music.enabled = status.Music;
            music.volume = status.MusicVolume;
        }
        foreach (AudioSource sfx in sfxs)
        {
            sfx.enabled = status.SFX;
            sfx.volume = status.SFXVolume;
        }
    }
    public void ToggleSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void changeSFX(Toggle on)
    {
        foreach (AudioSource sfx in sfxs)
        {
            sfx.enabled = on.isOn;
        }
        status.SFX = on.isOn;
    }

    public void changeMusic(Toggle on)
    {
        foreach (AudioSource music in musics)
        {
            music.enabled = on.isOn;
        }
        status.Music = on.isOn;
    }

    public void setMusicVolume(Slider vol)
    {
        foreach (AudioSource music in musics)
        {
            music.volume = vol.value;
        }
        status.MusicVolume = vol.value;
    }

    public void setSFXVolume(Slider vol)
    {
        foreach(AudioSource sfx in sfxs)
        {
            sfx.volume = vol.value;
        }
        status.SFXVolume = vol.value;
    }

}

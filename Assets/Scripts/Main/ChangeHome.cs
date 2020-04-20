using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHome : MonoBehaviour
{
    public List<Color> colors;
    private int curr_bckgd_color = 0;
    public List<Sprite> floor_objects;
    private int curr_floor_obj = 0;
    public List<Sprite> window_objects;
    private int curr_window_obj = 0;
    public Status status;
    public Image floor_object;
    public Image window_object;

    private void Start()
    {
        curr_bckgd_color = status.BackgroundColor;
        Camera.main.backgroundColor = colors[curr_bckgd_color];

        curr_floor_obj = status.FloorObject;
        floor_object.sprite = floor_objects[curr_floor_obj];

        curr_window_obj = status.WallObject;
        window_object.sprite = window_objects[curr_window_obj];
    }

    public void ChangeColor()
    {
        curr_bckgd_color = (curr_bckgd_color +  1) % colors.Count;
        Camera.main.backgroundColor = colors[curr_bckgd_color];
        status.BackgroundColor = curr_bckgd_color;
    }
    public void ChangeFloorObj()
    {
        curr_floor_obj = (curr_floor_obj + 1) % floor_objects.Count;
        floor_object.sprite = floor_objects[curr_floor_obj];
        status.FloorObject= curr_floor_obj;
    }
    public void ChangeWallObj()
    {
        curr_window_obj = (curr_window_obj + 1) % window_objects.Count;
        window_object.sprite = window_objects[curr_window_obj];
        status.WallObject = curr_window_obj;
    }
}

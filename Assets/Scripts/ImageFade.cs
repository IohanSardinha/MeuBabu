using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    // the image you want to fade, assign in inspector
    public Image img;
    public float time;
    public Color color;

    public void startFade(bool fade)
    {
        // fades the image out when you click
        StartCoroutine(FadeImage(fade, time, color));
    }


    IEnumerator FadeImage(bool fadeAway, float time, Color color, float alfa = 255)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = alfa; i >= 0; i -= Time.deltaTime/time)
            {
                // set color with i as alpha
                color.a = i;
                img.color = color;
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= alfa; i += Time.deltaTime/time)
            {
                // set color with i as alpha
                color.a = i;
                img.color = color;
                yield return null;
            }
        }
    }
}

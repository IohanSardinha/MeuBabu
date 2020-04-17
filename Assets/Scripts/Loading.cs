using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private Text text;
    private float time = 0;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time < 0.2)
        {
            text.text = "Carregando.";
        }
        else if(time < 0.4)
        {
            text.text = "Carregando..";
        }
        else if(time < 0.6)
        {
            text.text = "Carregando...";
        }
        else
        {
            time = 0;
        }
    }
}

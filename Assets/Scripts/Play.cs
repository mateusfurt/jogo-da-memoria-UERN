﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChamarJogoHum()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("jogo1");
    } 

}
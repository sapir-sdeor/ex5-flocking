using System;
using System.Collections;
using System.Collections.Generic;
using Flocking;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI peepText;
    public static int redScore;
    public static int blueScore;
    public static int groupWinner = 2;
    
   
    void Update()
    {
        redScore = 0;
        blueScore = 0;
        
        var peeps = FindObjectsOfType<PeepController>();
        foreach (var peep in peeps)
        {
            switch (peep.Group)
            {
                case 0:
                    redScore++;
                    break;
                case 1:
                    blueScore++;
                    break;
            }
        }

        peepText.text =  "<color=red>" + redScore + "<color=white>" + " : " + "<color=blue>" + blueScore;
       
    }

    public static void GameOver()
    {
        SceneManager.LoadScene("FinalScene");
    }

    
    
}
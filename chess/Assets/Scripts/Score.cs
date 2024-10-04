using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int WhiteScore = 0;
    public static int BlackScore = 0;
    public Text score;
    void Start()
    {
        score = GetComponent<Text> ();
    }

    
    void Update()
    {
        score.text = "White team | " + WhiteScore + " | " + BlackScore + " | BLack team";
    }
}

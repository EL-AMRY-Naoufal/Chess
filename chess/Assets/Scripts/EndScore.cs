using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndScore : MonoBehaviour
{
    public static string Info;
    public Text Winner;
    void Start()
    {
        Winner = GetComponent<Text>();
    }

    void Update()
    {
        Winner.text = "The winner is the " + Info;
    }
}

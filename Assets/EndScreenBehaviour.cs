using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenBehaviour : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = $"You lost - {(int) GameManager.instance.getScore()}$";
    }
}

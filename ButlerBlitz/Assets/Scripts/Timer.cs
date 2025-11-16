using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public float timer = 60;
    
    void Update()
    {
        timer -= Time.deltaTime;
        TimerText.text = "" + timer.ToString("f0");

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}

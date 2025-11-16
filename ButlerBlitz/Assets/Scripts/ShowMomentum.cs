using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ShowMomentum : MonoBehaviour
{
    public Image loadingImage;
    public TextMeshProUGUI momentumText;

    public MomentumScript momentumScript;
    public float acMomentum;
    

    void Update()
    {
        loadingImage.fillAmount = acMomentum / 100f;

        acMomentum = MomentumScript.Instance.ActualMomentum;
        momentumText.text = "" + acMomentum.ToString("f0");
    }
}

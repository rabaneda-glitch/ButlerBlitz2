using System;
using UnityEngine;

public class MomentumScript : MonoBehaviour
{
    public static MomentumScript Instance; // Singleton simple

    [Header("Cantidad Inicial")]
    [SerializeField] float qtyMmt = 10f;

    [Header("Decrecimiento")]
    [SerializeField] float decrAlto = 5f;
    [SerializeField] float decrBajo = 2.5f;
    [SerializeField] float mmtMinimo = 2f;

    [Header("Niveles de aumento")]
    public float bajoMmt = 10f;
    public float medioMmt = 20f;
    public float altoMmt = 30f;
    public float mAltoMmt = 40f;
    public float comboMmt = 15f;


    bool isDecreasing = true;
    [NonSerialized] public bool isWalking = false;
    float ultimoNivel = 0f,
    decr;
    public float ActualMomentum;

    void Awake()
    {
        // Asegurar una sola instancia
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isDecreasing)
            qtyMmt -= decr * Time.deltaTime;

        Decrecer();

        if (qtyMmt <= 0)
        {
            qtyMmt = 0;
            GameOver();
        }
        ActualMomentum = qtyMmt;
    }

    void Decrecer()
    {
        if (isWalking)
        {
            decr = decrBajo;

            if (qtyMmt <= mmtMinimo)
            {
                qtyMmt = mmtMinimo;

            }
        }
        else
        {
            decr = decrAlto;
            if (qtyMmt <= 0)
            {
                qtyMmt = 0;
            }
        }

    }

    public void Aumentar(float nivel)
    {
        qtyMmt += nivel;

        if (nivel > 10 && ultimoNivel > 10)
        {
            qtyMmt += comboMmt;
            Debug.Log("Combo +15pt");
        }

        ultimoNivel = nivel;

        if (qtyMmt > 100f)
            qtyMmt = 100f;
    }

    void GameOver()
    {
        Debug.Log("❌ Momentum = 0 → Game Over");
        // Aquí puedes pausar el juego, cambiar de escena, etc.
    }

    public float GetMomentum() => qtyMmt;
}





using UnityEngine;

public class Stain : MonoBehaviour
{
    public enum StainType { Mud, Dust, Grease, Water } // Tipos de manchas
    public StainType type = StainType.Mud; // Tipo de mancha por defecto

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Clean()
    {
        if (MomentumScript.Instance != null)
        {
            MomentumScript.Instance.Aumentar(MomentumScript.Instance.mAltoMmt); 
            Debug.Log("Tarea hecha: +40 momentum");
        }

        var prog = Object.FindFirstObjectByType<Progresion>();
        if (prog != null)
        {
            prog.IncrementStainsCleaned();
        }

        _renderer.enabled = false;
        Destroy(gameObject, 0.1f); //Tiempo que tarda en desaparecer la mancha
    }
}

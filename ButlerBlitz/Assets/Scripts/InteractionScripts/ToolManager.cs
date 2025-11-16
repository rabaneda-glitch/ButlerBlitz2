using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    public enum Tool { Vacuum = 1, Sponge = 2, Duster = 3, Mop = 4, Flush = 0}

    [Header("Referencias")]
    [SerializeField] private ToolChange toolChange; // Detecta la herramienta activa

    private Tool currentTool;
    public Tool CurrentTool => currentTool;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (toolChange == null) return;

        // Detectar herramienta actual desde ToolChange
        Tool newTool = (Tool)toolChange.SelectedTool;
        if (newTool != currentTool)
        {
            currentTool = newTool;
            Debug.Log($"Herramienta actual: {currentTool}");
        }
    }

    public bool IsCorrectToolFor(Stain stain)
    {
        if (stain == null) return false;

        return stain.type switch
        {
            Stain.StainType.Dust => currentTool == Tool.Duster,
            Stain.StainType.Grease => currentTool == Tool.Sponge,
            Stain.StainType.Water => currentTool == Tool.Mop,
            Stain.StainType.Mud => currentTool == Tool.Vacuum,
            _ => false
        };
    }
}

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Stain : MonoBehaviour
{
    public enum StainType { Mud, Dust, Grease, Water }
    public StainType type = StainType.Mud;

    [Header("Feedback")]
    [SerializeField] public float destroyDelay = 0.5f;

    private Renderer _renderer;
    private Collider _collider;

    [Header("VFX")]
    [SerializeField] private GameObject ParticleSystem;

    public AudioSource cleanSound;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();

    }

    public void Interact(GameObject interactor = null)
    {
        if (ToolManager.Instance != null)
        {
            bool correct = ToolManager.Instance.IsCorrectToolFor(this);
            if (!correct)
            {
                Debug.Log($"Herramienta incorrecta para {type}");
                return;
            }
        }

        StartClean();
    }

    void StartClean()
    {
        if (_collider != null) _collider.enabled = false;
        if (_renderer != null) _renderer.enabled = false;

        var prog = Object.FindFirstObjectByType<Progresion>();
        if (prog != null)
        {
            prog.IncrementStainsCleaned();
        }

        // Reproducir sonido
        if (cleanSound != null)
        {
            cleanSound.Play();
        }

        if (ParticleSystem != null)
        {
            //Sistema de particulas en la posición de la mancha
            GameObject vfxInstance = Instantiate(
                ParticleSystem,
                transform.position,
                Quaternion.identity
            );

            Destroy(vfxInstance, destroyDelay + 1f);
        }

        Destroy(gameObject, destroyDelay);
    }



}

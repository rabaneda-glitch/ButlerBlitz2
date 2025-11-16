using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CleanerRay : MonoBehaviour
{
    private Camera _camera;

    [Header("Ray Settings")]
    [SerializeField] private float maxDistance = 1000f;

    [Header("Cooldown")]
    [SerializeField] private float cooldownSecs = 1f;
    private float cooldown = 0f;

    [Header("FOV (Zoom)")]
    [SerializeField] private float fovZoom = 20f;
    private float fovOriginal;
    private float fovVelocity = 0f;

    [Header("Crosshair")]
    [SerializeField] private Texture2D crosshair;

    void Start()
    {
        _camera = GetComponent<Camera>();
        fovOriginal = _camera.fieldOfView;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- Zoom con botón derecho ---
        float targetFov = Input.GetMouseButton(1) ? fovZoom : fovOriginal;
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, targetFov, ref fovVelocity, 0.5f);

        // --- Cooldown ---
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        // --- Disparo con botón izquierdo ---
        if (Input.GetMouseButtonDown(0))
        {
            ShootRay();
            cooldown = cooldownSecs;
        }
    }

    private void ShootRay()
    {
        Vector3 center = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray ray = _camera.ScreenPointToRay(center);
        RaycastHit hit;

        int stainMask = LayerMask.GetMask("Stain");

        if (Physics.Raycast(ray, out hit, maxDistance, stainMask))
        {
            Stain stain = hit.transform.GetComponent<Stain>();
            if (stain != null)
            {
                stain.Interact(null);
            }
        }
    }

    void OnGUI()
    {
        if (crosshair == null) return;

        int size = 32;
        float posX = (_camera.pixelWidth - size) / 2;
        float posY = (_camera.pixelHeight - size) / 2;
        GUI.Label(new Rect(posX, posY, size, size), crosshair);
    }
}

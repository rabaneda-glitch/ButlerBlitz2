using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // obligatorio para IEnumerator

public class ToolChange : MonoBehaviour
{
    public int SelectedTool = 0;

    // Referencia al Animator que está en el HandPivot
    [SerializeField] private Animator handPivotAnimator;

    // Duraciones (ajusta a la duración real de tus clips)
    [SerializeField] private float hideDuration = 0.4f;
    [SerializeField] private float showDuration = 0.4f;

    // Pose visible inicial - asegúrate que coincide con la pose final de ToolShow
    [SerializeField] private Vector3 handPivotVisibleLocalPosition = Vector3.zero;

    private bool isChanging = false;

    void Start()
    {
        // 1) Asegurarnos de que el HandPivot aparece ya en la pose visible
        if (handPivotAnimator != null)
        {
            // Desactivar el Animator para que no ejecute animación al Start
            handPivotAnimator.enabled = false;

            // Forzar la posición visible inicial (localPosition del transform donde esté el Animator)
            // Nota: si tu Animator está en otro GameObject, ajusta la referencia:
            handPivotAnimator.gameObject.transform.localPosition = handPivotVisibleLocalPosition;
        }

        SelectTool();
    }

    void Update()
    {
        if (isChanging) return;

        int previousSelectedTool = SelectedTool;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedTool >= transform.childCount - 1)
                SelectedTool = 0;
            else
                SelectedTool++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedTool <= 0)
                SelectedTool = transform.childCount - 1;
            else
                SelectedTool--;
        }

        if (previousSelectedTool != SelectedTool)
        {
            StartCoroutine(ChangeToolAnimated(previousSelectedTool));
        }
    }

    IEnumerator ChangeToolAnimated(int previousTool)
    {
        isChanging = true;

        if (handPivotAnimator != null)
        {
            // Habilitar Animator justo antes de reproducir
            handPivotAnimator.enabled = true;

            // Reproducir directamente el estado de ocultar (esto salta a ese estado)
            handPivotAnimator.Play("ToolHide", 0, 0f);
        }

        // esperar a que "baje" la herramienta
        yield return new WaitForSeconds(hideDuration);

        // Desactivar herramienta anterior y activar la nueva (tu SelectTool hace esto)
        SelectTool();

        if (handPivotAnimator != null)
        {
            // Reproducir el estado de mostrar (la herramienta sube con la nueva activa)
            handPivotAnimator.Play("ToolShow", 0, 0f);
        }

        // esperar a que "suba" la herramienta
        yield return new WaitForSeconds(showDuration);
        transform.localPosition = Vector3.zero;

        // Desactivar el Animator otra vez para que no interfiera con la cámara o cause animación al volver a escena
        if (handPivotAnimator != null)
            handPivotAnimator.enabled = false;

        isChanging = false;
    }

    void SelectTool()
    {
        int i = 0;
        foreach (Transform tool in transform)
        {
            if (i == SelectedTool)
                tool.gameObject.SetActive(true);
            else
                tool.gameObject.SetActive(false);
            i++;
        }
    }
}

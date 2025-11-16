using UnityEngine;

public class HandPivotAnimator: MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayChangeAnimation()
    {
        anim.SetTrigger("Change");
    }
}

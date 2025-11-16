using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    public Transform TargetTransform;
    public Vector3 Offset;
    void Start()
    {
        transform.position = TargetTransform.position + Offset;
    }

   
}

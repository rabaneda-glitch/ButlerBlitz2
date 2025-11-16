using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform Target; 
    void Update()
    {
        transform.rotation = Target.rotation;
        
    }
}

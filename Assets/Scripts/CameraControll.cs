using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public float speed = 2.0f;
    public Transform target; 

    private void Awake()
    {
        if (!target)
        {
            target = FindFirstObjectByType<Character>().transform;
        }
    }

    private void LateUpdate()
    {
     
        Vector3 position = target.position;
        position.z = -10.0f; 

        
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
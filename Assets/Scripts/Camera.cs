using UnityEngine;

public class Camera : MonoBehaviour
{
    private float posY;

    private void Start()
    {
        posY = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}

using UnityEngine;

public class ParallaxBackgroundEffect : MonoBehaviour
{
    private float startPos;
    private float length;
    public float parallaxEffect;
    private float posY;

    public GameObject cam;

    private void Start()
    {
        startPos = transform.position.x;
        posY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, posY, transform.position.z);

        if (movement > startPos + length / 2)
        {
            startPos += length;
        }
        else if (movement < startPos - length / 2)
        {
            startPos -= length;
        }
    }
}

using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    [SerializeField] private HeroKnight heroKnight;

    private void Update()
    {
        if (heroKnight != null)
        {
            transform.position = new Vector3(heroKnight.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
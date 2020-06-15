using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WayPoint : MonoBehaviour
{
    public float hitboxRadius;
    private CircleCollider2D hitbox;

    public bool Active = true;

    public Vector3 position;

    private void Awake()
    {
        hitbox = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        hitbox.radius = hitboxRadius;
        //transform.position = position;
    }
}

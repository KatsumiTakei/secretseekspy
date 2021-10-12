using UnityEngine;
using UnityDLL;

public class Needle : MonoBehaviour
{
    BoxCollider2D boxCollider = null;

    public void Reset(Vector2 collisionSize, Vector2 offset)
    {
        boxCollider.size = collisionSize;
        boxCollider.offset = offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<IGunTarget>();
        if (target != null)
        {
            target.Hit();
            return;
        }
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}

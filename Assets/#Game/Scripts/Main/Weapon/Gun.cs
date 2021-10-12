
using UnityDLL;
using UnityEngine;
using DG.Tweening;

public interface IGunTarget
{
    void Hit();
}

public class Gun : MonoBehaviour
    , IWeapon
{

    [SerializeField]
    Needle needlePrefab = null;

    SpriteRenderer spriteRenderer = null;
    Transform parent = null;

    public void Use(Vector2 direction)
    {
        Anim(direction);

        var tilesize = TileMapManager.TileSize;
        var raydistance = TileMapManager.TileSize * 10f;

        var hit = Physics2D.RaycastAll(transform.position + tilesize * 0.5f * Vector3.down, direction, raydistance);
        //Debug.DrawRay(transform.position, direction * TileMapManager.TileSize * 10, Color.red, TileMapManager.TileSize * 10, false);

        Vector2 size = new Vector2(raydistance, raydistance) * direction.normalized;
        size = size.Clamp(new Vector2(tilesize, tilesize), new Vector2(raydistance, raydistance));

        foreach (var raycast in hit)
        {
            //print(raycast.collider.name);
            if (raycast.collider.gameObject.layer == 8)
            {
                var temp = (new Vector2(transform.position.x, transform.position.y) - raycast.point);
                size.x = Mathf.Clamp(Mathf.Abs(temp.x), tilesize, raydistance);
                size.y = Mathf.Clamp(Mathf.Abs(temp.y), tilesize, raydistance);
            }
        }

        var needle = Instantiate(needlePrefab, transform.position, Quaternion.identity);
        needle.Reset(size, direction.normalized * size * 0.5f);
        Destroy(needle.gameObject, 0.5f);
    }

    void Anim(Vector2 direction)
    {
        //  演出
        spriteRenderer.SetAlpha(1);
        spriteRenderer.DOFade(0f, 0.2f);

        parent.SetLocalEulerAnglesZ(direction.VectorToDeg());
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.SetAlpha(0);
    }

    private void Start()
    {
        parent = transform.parent;
    }

}
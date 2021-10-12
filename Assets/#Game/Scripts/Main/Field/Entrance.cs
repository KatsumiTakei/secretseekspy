using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using STLExtensiton;
using System;
using DG.Tweening;

public class Entrance : MonoBehaviour
    ,IInitializable
{
    [SerializeField]
    SpriteRenderer[] spriteRenderers = null;

    [SerializeField]
    eRoomDirection roomDirection = default;

    BoxCollider2D boxCollider = null;
    Room linkRoom = null;

    public Vector3 OriginLocalPos { get; private set; } = Vector3.zero;

    public void CloseShutter()
    {
        boxCollider.isTrigger = false;

        foreach (var spriteRenderer in spriteRenderers)
        {
            DOTween.To(
                () => spriteRenderer.size,
                (_) => spriteRenderer.size = _,
                new Vector2(32, 32),
                1f
            );
        }
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        OriginLocalPos = transform.localPosition;
    }

    public void Initialize()
    {
        if(!boxCollider)
        {
            Awake();
        }

        boxCollider.isTrigger = true;
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.size = new Vector2(32f, 0.1f);
        }

        RoomManager.Instance.LinkRooms(ref linkRoom, roomDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().MoveRoom();
            RoomManager.Instance.MoveRoom(transform.position, linkRoom);
        }
    }

}

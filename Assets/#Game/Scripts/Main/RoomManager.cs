using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDLL;
using System;
using STLExtensiton;

public enum eRoomDirection
{
    Down = MoveDirection.Down,
    Up = MoveDirection.Up,
    Left = MoveDirection.Left,
    Right = MoveDirection.Right,
    Main,
}

public class RoomManager : SingletonMonoBehaviour<RoomManager>
    , IInitializable
{
    [SerializeField]
    Room[] lrRooms = null;
    [SerializeField]
    Room[] rlRooms = null;
    [SerializeField]
    Room[] ubRooms = null;
    [SerializeField]
    Room[] buRooms = null;

    [SerializeField]
    Room mainMap = null;

    [SerializeField]
    SpriteRenderer darkness = null;

    Room currentRoom = null;

    Dictionary<eRoomDirection, List<Room>> vacantRooms = new Dictionary<eRoomDirection, List<Room>>();

    void OnGameFinish(bool isClear)
    {
        if (isClear)
            return;


    }


    public void Initialize()
    {
        vacantRooms = new Dictionary<eRoomDirection, List<Room>>()
        {
            { eRoomDirection.Down, new List<Room>(ubRooms) },
            { eRoomDirection.Up, new List<Room>(buRooms) },
            { eRoomDirection.Left, new List<Room>(rlRooms) },
            { eRoomDirection.Right, new List<Room>(lrRooms) },
        };

        if(currentRoom && currentRoom != mainMap)
        {
            currentRoom.Exit();
        }

        currentRoom = mainMap;
        mainMap.gameObject.SetActive(true);

        var entrances = FindObjectsOfType<Entrance>();
        foreach (var entrance in entrances)
        {// メインマップの接続
            entrance.Initialize();
        }

        initialize(ubRooms);
        initialize(buRooms);
        initialize(rlRooms);
        initialize(lrRooms);

        void initialize(Room[] rooms)
        {// サブマップの接続
            foreach (var room in rooms)
            {
                room.GetComponentInChildren<Entrance>(true).Initialize();
            }
        }

    }


    public void LinkRooms(ref Room room, eRoomDirection direction)
    {
        var rooms = vacantRooms.TryGetValueEx(direction, null);
        if (rooms == null || rooms.Count == 0)
        {
            room = mainMap;
            return;
        }

        Debug.Assert(rooms?.Count > 0, "rooms?.Count > 0");

        var index = UnityEngine.Random.Range(0, rooms.Count);
        room = rooms[index];
        rooms.RemoveAt(index);
    }

    public void MoveRoom(Vector3 entrancePos, Room room)
    {
        AudioManager.PlaySE("MoveRoom");
        currentRoom.Exit();
        currentRoom = room;

        bool isMainMap = currentRoom.Equals(mainMap);
        room.Admission(entrancePos, isMainMap);
    }

    void OnEnable()
    {
        EventManager.OnGameFinish += OnGameFinish;
    }

    void OnDisable()
    {
        EventManager.OnGameFinish -= OnGameFinish;
    }
}

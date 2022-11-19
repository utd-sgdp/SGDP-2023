using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Camera;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Level
{
    public class Room : MonoBehaviour
    {
        [Header("Meta Data")]
        [SerializeField]
        [Tooltip("Meta data about the type of room.")]
        RoomTag _tags;

        [Header("References")]
        [SerializeField]
        RoomTrigger _trigger;
        
        [SerializeField]
        [NonReorderable]
        RoomDoor[] _doors;

        bool _hasEntered;
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            _trigger = GetComponentInChildren<RoomTrigger>();
        }
        #endif

        void OnEnable()
        {
            _trigger.OnRoomEnter.AddListener(OnEnter);
        }

        void OnDisable()
        {
            _trigger.OnRoomEnter.RemoveListener(OnEnter);
        }

        [Button(Label="Collect Doors")]
        void DetectDoors()
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Auto-detect Room Doors");
            #endif

            // get doors from children
            Door[] doors = GetComponentsInChildren<Door>();
            _doors = doors.Select(door => new RoomDoor { door = door }).ToArray();
            
            // assign directions to each door
            SortDoors(_doors);
        }

        static void SortDoors(IEnumerable<RoomDoor> doors)
        {
            // List<RoomDoor> list = doors.ToList();
            
            // west
            // list.Sort((a, b) => a.door.transform.position.x.CompareTo(b.door.transform.position.x));
            // if (list[0].direction == DoorDirection.Null)
            // {
            //     list[0].direction = DoorDirection.North;
            //     list[0].direction = DoorDirection.West;
            // }
            //
            // // east
            // list.Sort((a, b) => -a.door.transform.position.x.CompareTo(b.door.transform.position.x));
            // if (list[0].direction == DoorDirection.Null)
            // {
            //     list[0].direction = DoorDirection.East;
            // }
            //
            // // south
            // list.Sort((a, b) => a.door.transform.position.z.CompareTo(b.door.transform.position.z));
            // if (list[0].direction == DoorDirection.Null)
            // {
            // }
            //
            // // north
            // list.Sort((a, b) => -a.door.transform.position.z.CompareTo(b.door.transform.position.z));
            // if (list[0].direction == DoorDirection.Null)
            // {
            //     list[0].direction = DoorDirection.South;
            // }
        }

        #region Entrance Behaviour
        void OnEnter()
        {
            // exit, this room has already been entered
            if (_hasEntered) return;

            _hasEntered = true;
            StartCoroutine(CloseDoors());
        }

        IEnumerator CloseDoors()
        {
            foreach (RoomDoor d in _doors)
            {
                d.door.Close();
            }
            
            yield return new WaitForSeconds(5);

            foreach (RoomDoor d in _doors)
            {
                d.door.Open();
            }
        }
        #endregion
    }
    
    #region Data
    [System.Flags]
    public enum RoomTag
    {
        Combat = 1,
        Buffer = 2,
        Loot = 4,
        Boss = 8,
    }

    public enum DoorDirection
    {
        Null = 0,
        North = 1,
        South = 2,
        West = 3,
        East = 4,
    }

    [Serializable]
    public class RoomDoor
    {
        public Door door;
        public DoorDirection direction;
    }
    #endregion
}
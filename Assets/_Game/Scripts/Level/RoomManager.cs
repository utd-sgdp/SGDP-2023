using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(BoxCollider))]
    public class RoomManager : MonoBehaviour
    {
        [SerializeField]
        RoomTag[] roomTags;

        [SerializeField]
        BoxCollider footprint;

        [SerializeField]
        [NonReorderable]
        RoomDoor[] roomDoors;

        

        bool hasEntered = false;

        private void Awake()
        {
            if(footprint != null)
                footprint.enabled = false;
        }

        #region Detect Doors
        [Button]
        void DetectDoorDirection()
        {
            for (int i = 0; i<roomDoors.Length; i++)
            {
                roomDoors[i].direction = getDirection(roomDoors[i].door);
                
            }
        }

        private DoorDirection getDirection(Door door)
        {
            float northDist = Math.Abs(door.transform.position.z - footprint.center.z - footprint.size.z);
            float southDist = Math.Abs(door.transform.position.z - footprint.center.z + footprint.size.z);
            float westDist = Math.Abs(door.transform.position.x - footprint.center.x + footprint.size.x);
            float eastDist = Math.Abs(door.transform.position.x - footprint.center.x - footprint.size.x);

            float minDist = new[] { northDist, southDist, westDist, eastDist }.Min();
            Debug.Log(minDist);

            if (minDist == northDist)
                return DoorDirection.North;

            if (minDist == southDist)
                return DoorDirection.South;

            if (minDist == westDist)
                return DoorDirection.West;

            return DoorDirection.East;
        }
        #endregion

        #region Entrance Behaviour
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                onEnter();
        }

        private void onEnter()
        {
            if (!hasEntered)
            {
                hasEntered = true;
                StartCoroutine(CloseDoors());
            }
        }

        IEnumerator CloseDoors()
        {
            foreach (RoomDoor d in roomDoors)
                d.door.Close();
            yield return new WaitForSeconds(5);
            foreach (RoomDoor d in roomDoors)
                d.door.Open();
        }
        #endregion
    }

    #region Structs and Enums
    public enum RoomTag
    {
        Test,
        Challenge,
        Reward,
        Shop
    }

    public enum DoorDirection
    {
        North,
        South,
        West,
        East
    }

    [Serializable]
    public struct RoomDoor
    {
        [SerializeField]
        public Door door;

        [SerializeField]
        public DoorDirection direction;
    }
    #endregion
}

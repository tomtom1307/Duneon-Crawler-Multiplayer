using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CorridorRoom : MonoBehaviour
    {
        public GameObject UpWall;
        public GameObject DownWall;
        public GameObject LeftWall;
        public GameObject RightWall;
        public List<GameObject> GOS = new List<GameObject>();



        public enum WallTypes{
            Empty = 0,
            Wall = 1,
            Door = 2
        }


        private void Awake()
        {
            GOS = new List<GameObject>{ UpWall, DownWall, LeftWall, RightWall};
        }


        public void DoWall(int index)
        {
            print(GOS.Count);
            GOS[index].gameObject.SetActive(true);
        }

        public void DoEmpty (int index)
        {
            GOS[index].gameObject.SetActive(false);
        }

        public void DoDoor(int index)
        {
            GOS[index].gameObject.SetActive(false);
        }


    }
}

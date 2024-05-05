using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project
{
    [Serializable]
    public class family {

        public List<int> rooms;
        public family(List<int> rooms)
        {
            this.rooms = rooms; 
        }

        public void mergeFams(family fam, List<family> families)
        {
            rooms = rooms.Union(fam.rooms).ToList();
            families.Remove(fam);
        }


           
    }
}

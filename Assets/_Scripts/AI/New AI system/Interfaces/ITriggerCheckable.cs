using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public interface ITriggerCheckable
    {
        bool IsWithinStrikingDistance { get; set; }
        

        void SetStrikingDistanceBool(bool isStriking);
    }
}

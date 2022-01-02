using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

   
    public class ArmourClass : MonoBehaviour
    {
        [SerializeField] int armourClassValue = 0;


        public int ArmourClassValue {  get { return armourClassValue; } }

        public int CalculateArmourClass()
        {
            return armourClassValue;
        }

    }

}

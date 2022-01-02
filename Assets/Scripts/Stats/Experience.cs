using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;


        public float ExperiencePoints
        {
            get { return experiencePoints; }
        }


        //public delegate void ExperianceGainedDelegate();
        public event Action onExperiencedGained; //Using Action means I don't need to the delegate defined above

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperiencedGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}



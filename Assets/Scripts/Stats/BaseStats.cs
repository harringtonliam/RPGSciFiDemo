using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 5)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpPrefab = null;
        
        public event Action onLevelUp;
        
        int currentLevel = 0;
        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperiencedGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperiencedGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpPrefab != null)
            {
                Instantiate(levelUpPrefab, transform);
            }
        }

        public float GetStat(Stat stat)
        {


            int level = GetLevel();
            //float additiveModifier = GetAdditiveModifier(stat);
            //float percentageModifier = GetPercenatgeModifier(stat);


            return (progression.GetStat(stat, characterClass, level));// + additiveModifier) * (1+ (percentageModifier/100));
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float statModifier = 0f;

            IModiifierProvider[] modiifierProviders = GetComponents<IModiifierProvider>();
            foreach (var modifierProvider in modiifierProviders)
            {
                foreach (var modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    statModifier += modifier;
                }
            }

            return statModifier;
        }

        private float GetPercenatgeModifier(Stat stat)
        {
            float statModifier = 0f;

            IModiifierProvider[] modiifierProviders = GetComponents<IModiifierProvider>();
            foreach (var modifierProvider in modiifierProviders)
            {
                foreach (var modifier in modifierProvider.GetPercentageModifiers(stat))
                {
                    statModifier += modifier;
                }
            }

            return statModifier;
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null)
            {
                return startingLevel;
            }


            float currentXP = GetComponent<Experience>().ExperiencePoints;
            int maxLevels  = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = maxLevels; level >= 1; --level)
            {
                //if (gameObject.tag == "Player")
                //{
                //    Debug.Log("GetLevel loop " + level + " current xp=" + currentXP + " level xp="  + progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level));   
                //}
                 
                if(currentXP >= progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level))
                {
                    return level;
                }
            }
            return 1;

        }



    }

}

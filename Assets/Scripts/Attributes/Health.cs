using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine.Events;

namespace RPG.Attributes
{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;


        float healthPoints = -1f;

        bool isDead = false;

        public bool IsDead { get { return isDead; } }

        public float HealthPoints {
            get { return healthPoints; }
        }

        void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
         }

        private void OnEnable()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            if (baseStats != null)
            {
                baseStats.onLevelUp += BaseStats_onLevelUp;
            }
        }

        private void OnDisable()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            if (baseStats != null)
            {
                baseStats.onLevelUp -= BaseStats_onLevelUp;
            }
        }

        public float GetPercentage()
        {
            return ( healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        public float GetMaxHealthPoints()
        {

            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints <= 0)
            {
                AwardExperience(instigator);
                Die();

            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healing)
        {
            healthPoints = Mathf.Min(healthPoints + healing, GetMaxHealthPoints());
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (instigator.GetComponent<Experience>() != null && !isDead)
            {
                float experienceGained = gameObject.GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
                experience.GainExperience(experienceGained);
            }
        }



        public void Die()
        {
            if (isDead) return; 
            onDie.Invoke();
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("die");
            } 
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void BaseStats_onLevelUp()
        {
            float newHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            healthPoints = newHealthPoints;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                onDie.Invoke();
                Die();
            }
        }
    }
}

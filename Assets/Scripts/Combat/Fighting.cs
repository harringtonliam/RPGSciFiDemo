using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using RPG.InventoryControl;

namespace RPG.Combat
{
    public class Fighting : MonoBehaviour, IAction, ISaveable
    {


        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeaponConfig = null;
        [SerializeField] Health target;

        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        Weapon currentWeapon;
        Equipment equipment;


        private void Awake()
        {
            equipment = GetComponent<Equipment>();

            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (currentWeaponConfig == null)
            {
               currentWeapon = EquipWeapon(defaultWeaponConfig);
            }
            
        }



        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            Mover mover = GetComponent<Mover>();

            if (target != null)
            {
                if (target.IsDead)
                {
                    return;
                }

                if(mover != null) mover.MoveTo(target.transform.position, 1f);
                if (GetIsInRange(target.transform))
                {
                    if(mover != null) mover.Cancel();
                   
                    AttackBehaviour();
                }
            }
        }

        public Health GetTarget()
        {
            return target;
        }


        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            Animator animator = GetComponent<Animator>();
            currentWeapon = currentWeaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
            return currentWeapon;
        }

        private void UpdateWeapon()
        {
            var weaponConfig = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weaponConfig != null)
            {
                EquipWeapon(weaponConfig);
            }
            else
            {
                EquipWeapon(defaultWeaponConfig);
            }

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Events
        public void Hit()
        {
            if (target == null) return;
            
            if (currentWeapon != null)
            {
                currentWeapon.OnHit();
            }
           
            if (currentWeaponConfig.IsRangedWeapon && !CheckLineOfSite())
            {
                return;
            }

            float calculatedDamage = 0;
            

            if (AttackRollSuccessful())
            {
                calculatedDamage = currentWeaponConfig.CalcWeaponDamage() + GetComponent<CharacterAbilities>().GetAbilityModifier(currentWeaponConfig.ModifierAbility);
            }


            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, calculatedDamage);
            }
            else
            { 
                target.TakeDamage(calculatedDamage, gameObject);
            }
        }

        private bool CheckLineOfSite()
        {
            if (target == null) return false;

            Vector3 lineOfSightStart = currentWeaponConfig.GetTransform(rightHandTransform, leftHandTransform).position;
            Vector3 lineOfSightEnd = GetAimLocation();

            Vector3 lineOfSiteDirection = (lineOfSightEnd - lineOfSightStart).normalized;
    
            Ray ray = new Ray(lineOfSightStart, lineOfSiteDirection);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, currentWeaponConfig.WeaponRange);
            Health targetHealth = hit.transform.GetComponent<Health>();

            if (targetHealth == null || targetHealth != target)
            {
                return false;
            }
           
            return true;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsuleCollider = target.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * capsuleCollider.height / 2;
        }

 

        private bool AttackRollSuccessful()
        {
            int targetArmourClass = target.GetComponent<ArmourClass>().CalculateArmourClass();
            int diceRoll = FindObjectOfType<Dice>().RollDice(20, 1);
            if (diceRoll >= 20) return true;
            if (diceRoll <= 1) return false;
            int attackRoll = diceRoll + currentWeaponConfig.WeaponToHitBonus + GetStatModifier();
            //if (attackRoll < targetArmourClass)
            //{
            //    Debug.Log(gameObject + " missed their attack");
            //}

            return attackRoll >= targetArmourClass;
        }

        private int GetStatModifier()
        {
            CharacterAbilities characterAbilities = GetComponent<CharacterAbilities>();
            if (characterAbilities == null) return 0;

            return characterAbilities.GetAbilityModifier(currentWeaponConfig.ModifierAbility);

        }

        public void Shoot()
        {
            Hit();
        }
        //End of animation events

        private bool GetIsInRange(Transform targetTransform)
        {
            return currentWeaponConfig.WeaponRange >= Vector3.Distance(targetTransform.position, transform.position);
        }

        public void Attack(GameObject combatTarget)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>(); ;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;


            if (GetComponent<Mover>() != null)
            {
                if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position)
                        && !GetIsInRange(combatTarget.transform))
                {
                    return false;
                }
            }

            if (GetComponent<Mover>() == null)
            {
                if (!GetIsInRange(combatTarget.transform))
                {
                    return false;
                }
            }



            Health targetHealth = combatTarget.GetComponent<Health>();
            if (targetHealth.IsDead)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            if (GetComponent<Mover>() != null)
            {
                GetComponent<Mover>().Cancel();
            }
           
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            if (currentWeaponConfig!= null)
            {
                return currentWeaponConfig.name;
            }
            else
            {
                return null;
            }
    
        }

        public void RestoreState(object state)
        {
            if (state != null)
            {
                string weaponName = (string)state;
                WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
                EquipWeapon(weapon);
            }

        }

    }
}
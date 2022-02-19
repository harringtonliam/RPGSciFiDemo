using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        [SerializeField] bool isActive = true;

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            Fighting fighting = playerController.transform.GetComponent<Fighting>();
            if (fighting.CanAttack(gameObject)  && isActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    fighting.Attack(gameObject);
                }
                return true;
            }
            return false;
        }
    }
}
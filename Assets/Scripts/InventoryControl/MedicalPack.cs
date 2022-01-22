using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Core;

namespace RPG.InventoryControl
{
    [CreateAssetMenu(menuName = ("Items/MedicalPack"))]
    public class MedicalPack :  ActionItem
    {
        [SerializeField] int medicalPackHealingDice = 8;
        [SerializeField] int medicalPackHealingDiceNumber = 1;
        [SerializeField] int medicalPackHealingAdditiveBonus = 0;

        public override void Use(GameObject user)
        {
            Health health = user.GetComponent<Health>();
            if (health == null) return;

            Dice dice = FindObjectOfType<Dice>();

            int healingAmount = dice.RollDice(medicalPackHealingDice, medicalPackHealingDiceNumber) + medicalPackHealingAdditiveBonus;

            health.Heal(healingAmount);

        }
    }
}



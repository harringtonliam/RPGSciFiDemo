using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;

namespace RPG.Combat
{
    public class ArmourClass : MonoBehaviour
    {
        [SerializeField] int baseArmourClass = 10;

        int maxDexBonus = 20;

        public int BaseArmourClass {  get { return baseArmourClass; } }

        public int CalculateArmourClass()
        {
            int dexterityModifier = GetDexterityModifier();
            int armourVakue = GetWornAmourBonus();

            if (dexterityModifier > maxDexBonus)
            {
                dexterityModifier = maxDexBonus;
            }

            return armourVakue + dexterityModifier;
        }

        private int GetDexterityModifier()
        {
            CharacterAbilities characterAbilities = GetComponent<CharacterAbilities>();
            if (characterAbilities == null) return 0;

            return characterAbilities.GetAbilityModifier(Ability.Dexterity);
        }

        private int GetWornAmourBonus()
        {
            int calculatedArmourValue = 0;
            Equipment equipment = GetComponent<Equipment>();
            if (equipment == null)
            {
                return calculatedArmourValue;
            }

            foreach (var item in equipment.GetEquippedItems())
            {
                Armour armour = item.Value as Armour;
                if (armour != null)
                {
                    calculatedArmourValue += armour.ArmourClassBonus;
                    if (maxDexBonus > armour.MaxDexBonus)
                    {
                        maxDexBonus = armour.MaxDexBonus;
                    }
                }
            }

            return calculatedArmourValue;
        }

    }

}

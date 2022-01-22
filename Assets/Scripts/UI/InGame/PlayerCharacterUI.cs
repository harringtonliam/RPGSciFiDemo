using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.UI.InGame
{
    public class PlayerCharacterUI : MonoBehaviour
    {
        [SerializeField] RectTransform foregroundHeealthBar = null;

        GameObject playerCharacterGameObject = null;
        Health health = null;


        string characterName = null;

        public void SetUp(GameObject newPlayerCharacterGameObject)
        {
            var iconImage = GetComponent<Image>();
            playerCharacterGameObject = newPlayerCharacterGameObject;

            CharacterSheet characterSheet = playerCharacterGameObject.GetComponent<CharacterSheet>();
            if (characterSheet != null)
            {
                iconImage.sprite = characterSheet.Portrait;
                iconImage.enabled = true;
                characterName = characterSheet.name;
            }

            health = playerCharacterGameObject.GetComponent<Health>();
        }

        void Update()
        {
            if (health == null) return;
            if (foregroundHeealthBar == null) return;
            foregroundHeealthBar.localScale = new Vector3(health.HealthPoints / health.GetMaxHealthPoints(), 1, 1);
        }

        private float GetHealthFraction()
        {
            return health.HealthPoints / health.GetMaxHealthPoints();
        }
    }



}


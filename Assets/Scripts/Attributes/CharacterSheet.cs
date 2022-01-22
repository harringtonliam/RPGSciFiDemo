using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Attributes
{
    public class CharacterSheet : MonoBehaviour
    {
        [SerializeField] string characterName = "No Name";
        [SerializeField] Sprite portrait = null;


        public string CharatcerName { get { return characterName; } }
        public Sprite Portrait { get { return portrait; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}



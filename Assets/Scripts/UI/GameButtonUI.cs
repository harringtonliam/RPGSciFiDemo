using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.UI
{
    public class GameButtonUI : MonoBehaviour
    {
        [SerializeField] ShowHideUI uiToShow;

        public void ButtonClicked()
        {
            ShowHideUI[] allshowHides = FindObjectsOfType<ShowHideUI>();
            for (int i = 0; i < allshowHides.Length; i++)
            {
                if (allshowHides[i] != uiToShow)
                {
                    allshowHides[i].SetUiActive(false);
                }

            }



            if (uiToShow != null)
            {
                uiToShow.ToggleUI();
            }
        }
    }
}


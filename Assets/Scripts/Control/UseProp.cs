using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;


namespace RPG.Control
{
    public class UseProp : MonoBehaviour, IAction
    {
        [SerializeField] float useRange = 1f;


        UseableProp target;

        // Update is called once per frame
        void Update()
        {
            Mover mover = GetComponent<Mover>();

            if (target != null)
            {
                mover.MoveTo(target.transform.position, 1f); ;
                if (GetIsInRange())
                {
                    mover.Cancel();

                    UseAblePropBehaviour();
                }
            }
        }

        private void UseAblePropBehaviour()
        {
            transform.LookAt(target.transform);
            target.UseProp();
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public void StartUseProp(GameObject useAbleProp)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = useAbleProp.GetComponent<UseableProp>(); ;
        }


        public void Cancel()
        {
            target = null;
            GetComponent<Mover>().Cancel();
        }



        private bool GetIsInRange()
        {
            bool isInRange = useRange >= Vector3.Distance(target.transform.position, transform.position);
            return isInRange;
        }
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA.UI
{
    // ==================================================================================================== CardUIHandler

    public abstract class CardUIHandler : UIHandler
    {
        //

        //

        private Transform _container;

        //

        //

        public Transform Container
        {
            get => _container;

            private set => _container = value;
        }

        //

        //

        public abstract override void Refresh();
    } 
}

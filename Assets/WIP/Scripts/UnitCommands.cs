using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== UnitCommand

    public abstract class UnitCommand<TUnit, TUnitObject> : ICommand<TUnit, TUnitObject> where TUnit : IUnit where TUnitObject : IUnitObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        public TUnitObject _view;

        public TUnit _controller;

        // ==================================================================================================== Property

        // =========================================================================== Instance

        public TUnitObject View
        {
            get
            {
                return _view;
            }

            set
            {
                _view = value;
            }
        }

        public TUnit Controller
        {
            get
            {
                return _controller;
            }

            set
            {
                _controller = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Command

        public abstract void Invoke();
    }

    // ==================================================================================================== CardRefreshCommand

    public class CardRefreshCommand : UnitCommand<Card, CardObject>
    {
        // ==================================================================================================== Property

        // =========================================================================== Instance

        public override void Invoke()
        {
            View.Refresh(Controller);
        }
    }

    // ==================================================================================================== CardArrangeCommand

    public class CardArrangeCommand : UnitCommand<Card, CardObject>
    {
        // ==================================================================================================== Property

        // =========================================================================== Instance

        public override void Invoke()
        {
            int count = CardManager.Instance.MyHandCards.Count - 1;
            int index = CardManager.Instance.MyHandCards.IndexOf(Controller);

            View.Arrange(count, index);
        }
    }

    // ==================================================================================================== ICommand

    public interface ICommand<TController, TView> where TController : IInstance where TView : IInstance
    {
        // ==================================================================================================== Property

        // =========================================================================== Instance

        public TView View
        {
            get; set;
        }

        public TController Controller
        {
            get; set;
        }

        // ==================================================================================================== Method

        // =========================================================================== Command

        public void Invoke();
    }
}

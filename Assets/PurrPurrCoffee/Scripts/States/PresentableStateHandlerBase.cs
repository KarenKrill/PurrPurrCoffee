using System;
using System.Linq;

using KarenKrill.StateSystem.Abstractions;
using KarenKrill.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.States
{
    public abstract class PresentableStateHandlerBase<T> : IStateHandler<T> where T : Enum
    {
        public abstract T State { get; }

        public PresentableStateHandlerBase(params IPresenter[] presenters)
        {
            _presenters = presenters.ToArray();
        }
        public virtual void Enter(T prevState)
        {
            foreach (var presenter in _presenters)
            {
                presenter.Enable();
            }
        }
        public virtual void Exit(T nextState)
        {
            foreach (var presenter in _presenters)
            {
                presenter.Disable();
            }
        }

        private readonly IPresenter[] _presenters;
    }
}

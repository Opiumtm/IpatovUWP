using System;
using System.Collections.Generic;

namespace Ipatov.Async
{
    /// <summary>
    /// Action list.
    /// </summary>
    internal struct ActionList : IDisposable
    {
        private readonly List<Action> _actions;

        public ActionList(List<Action> actions)
        {
            _actions = actions ?? new List<Action>();
        }

        /// <summary>
        /// Register action.
        /// </summary>
        /// <param name="a">Action.</param>
        public void Register(Action a)
        {
            if (a != null)
            {
                _actions.Add(a);
            }
        }

        /// <summary>
        /// Execute actions.
        /// </summary>
        public void Execute()
        {
            foreach (var a in _actions)
            {
                a();
            }
        }

        /// <summary>
        /// Implicit operator.
        /// </summary>
        /// <param name="actions">Source actions.</param>
        public static implicit operator ActionList(List<Action> actions)
        {
            return new ActionList(actions);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Execute();
        }
    }
}
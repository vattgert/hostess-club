using System.Collections.Generic;
using UnityEngine;

namespace GamesCore.ObservableSubjects
{
    public abstract class ObservableContextBase : MonoBehaviour
    {
        public delegate void ContextDestroyed();

        public event ContextDestroyed OnContextDestroyed;

        public void Unsubscribe()
        {
            OnContextDestroyed?.Invoke();
        }
    }
}
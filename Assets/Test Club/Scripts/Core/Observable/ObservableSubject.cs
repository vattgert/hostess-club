using System;
using System.Collections.Generic;

namespace GamesCore.ObservableSubjects
{
    public interface ISubject<T>
    {
        void Subscribe(ObservableContextBase context, Action<T> method);
        T Value { get; }
    }

    public abstract class SubjectBase<T> : ISubject<T>
    {
        protected T value;

        protected List<Action<T>> observers;
        protected ObservableContextBase currentContext;
        
        public void Subscribe(ObservableContextBase context, Action<T> method)
        {
            observers.Add(method);
            currentContext = context;
            currentContext.OnContextDestroyed += OnContextDestroyed;
        }
        
        private void OnContextDestroyed()
        {
            observers = null;
        }
        
        protected void Update()
        {
            if (observers != null)
            {
                foreach (Action<T> observer in observers)
                {
                    observer.Invoke(Value);
                }
            }
        }
        
        public abstract T Value { get; protected set; }

    }

    public class Subject<T> : SubjectBase<T>
    {
        public Subject()
        {
            observers = new List<Action<T>>();
        }
        
        public new void Subscribe(ObservableContextBase context, Action<T> method)
        {
            base.Subscribe(context, method);
        }

        public override T Value
        {
            get => value;
            protected set
            {
                base.value = value;
                Update();
            }
        }

        public void Emit(T data)
        {
            Value = data;
        }

    }

    public class ObservableSubject<T> : SubjectBase<T> where T : IEquatable<T>
    {
        public ObservableSubject()
        {
            observers = new List<Action<T>>();
        }

        public void SetValue(T newValue)
        {
            Value = newValue;
        }
        
        public override T Value
        {
            get => value;
            protected set
            {
                if (this.value == null)
                {
                    this.value = value;
                    Update();
                }
                else if (!this.value.Equals(value))
                {
                    this.value = value;
                    Update();
                }
            }
        }
    }
}
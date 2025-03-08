using UnityEngine;

namespace GamesCore.ObservableSubjects
{
    public class ChildObservableContext : ObservableContextBase
    {
        [SerializeField] private RootObservableContext rootContext;

        private void Start()
        {
            if (rootContext != null)
            {
                rootContext.AddContext(this);
            }
        }
        
        private void OnDestroy()
        {
            if (rootContext == null)
            {
                Unsubscribe();
            }
        }
    }
}
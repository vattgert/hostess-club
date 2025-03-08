using System.Collections.Generic;
using UnityEngine;

namespace GamesCore.ObservableSubjects
{
    public class RootObservableContext : ObservableContextBase
    {
        private List<ObservableContextBase> connectedContexts;

        private void Awake()
        {
            connectedContexts = new List<ObservableContextBase>();
        }

        public void AddContext(ObservableContextBase context)
        {
            connectedContexts.Add(context);
        }
        
        private void OnDestroy()
        {
            foreach (ObservableContextBase context in connectedContexts)
            {
                if (context != null)
                {
                    context.Unsubscribe();
                }
            }
        }

    }
}

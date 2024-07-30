using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace Core.Controllers.Core.CompositeDisposable
{
    public class ControllerCompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables;

        public ControllerCompositeDisposable()
        {
            _disposables = ListPool<IDisposable>.Get();
        }

        /// <summary>
        /// Adds a disposable object to the internal list of disposables.
        /// This method is used to keep track of all disposable objects that the ControllerCompositeDisposable is responsible for disposing.
        /// </summary>
        /// <param name="disposable">The disposable object to add to the list.</param>
        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        /// <summary>
        /// Adds a collection of disposable objects to the internal list of disposables.
        /// This method is used to keep track of all disposable objects that the ControllerCompositeDisposable is responsible for disposing.
        /// </summary>
        /// <param name="collection">The collection of disposable objects to add to the list.</param>
        public void AddRange(IEnumerable<IDisposable> collection)
        {
            _disposables.AddRange(collection);
        }

        public void Dispose()
        {
            using var pooledObject = ListPool<Exception>.Get(out var exceptionList);

            foreach (var disposable in _disposables)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception e)
                {
                    exceptionList.Add(e);
                }
            }

            _disposables.Clear();

            ListPool<IDisposable>.Release(_disposables);

            if (exceptionList.Any())
            {
                throw new AggregateException(exceptionList);
            }
        }
    }
}

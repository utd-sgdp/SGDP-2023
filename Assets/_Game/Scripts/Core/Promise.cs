// This is my twist on Real-Serious-Games' C-sharp-Promise library
// https://github.com/Real-Serious-Games/C-Sharp-Promise/blob/master/src/Promise_NonGeneric.cs
//
// Mine is designed specifically for Unity and the use of Action<Promise> instead of Func<Promise>
// is what drives most differences. My system creates promises and connects them for you so
// the user's functions don't have to create promises. They take a promise and update as necessary.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Promise
    {
        #region Meta-Data
        public string Name { get; private set; }

        public Promise WithName(string name)
        {
            Name = name;
            return this;
        }
        #endregion

        #region Data
        public PromiseState State { get; private set; } = PromiseState.Pending;
        public Exception RejectionException { get; private set; }

        readonly LinkedList<Action> _fulfillCallbacks = new();
        readonly LinkedList<Action<Exception>> _rejectCallbacks = new();
        readonly LinkedList<Action<float>> _progressCallbacks = new();
        #endregion

        #region Construtors
        public Promise(string name = null)
        {
            Name = name;
        }
        #endregion

        #region Handlers
        public void AddFulfillHandler(Action callback)
        {
            if (State is PromiseState.Fulfilled)
            {
                callback?.Invoke();
                return;
            }
            else if (State is PromiseState.Rejected) return;

            _fulfillCallbacks.AddLast(callback);
        }

        public void AddRejectHandler(Action<Exception> callback)
        {
            if (State is PromiseState.Rejected)
            {
                callback?.Invoke(RejectionException);
                return;
            }
            else if (State is PromiseState.Fulfilled) return;

            _rejectCallbacks.AddLast(callback);
        }

        public Promise AddProgressHandler(Action<float> callback)
        {
            if (State.IsSettled()) return this;

            _progressCallbacks.AddLast(callback);
            return this;
        }

        void InvokeFulfilled()
        {
            foreach (Action onFulfill in _fulfillCallbacks)
            {
                onFulfill?.Invoke();
            }
        }

        void InvokeRejection(Exception exception)
        {
            foreach (Action<Exception> onReject in _rejectCallbacks)
            {
                onReject?.Invoke(exception);
            }
        }

        void InvokeProgress(float amount)
        {
            foreach (Action<float> onProgress in _progressCallbacks)
            {
                onProgress?.Invoke(amount);
            }
        }
        #endregion

        #region Update
        public void Fulfill()
        {
            if (State.IsSettled())
            {
                throw new PromiseStateException(
                    message:
                    "Cannot fulfill promise" +
                    (Name != null ? $" {Name}" : "") +
                    ". It has already been settled."
                );
            }

            State = PromiseState.Fulfilled;
            InvokeFulfilled();
        }

        public void Reject(Exception exception)
        {
            if (State.IsSettled())
            {
                throw new PromiseStateException(
                    message:
                    "Cannot fulfill promise" +
                    (Name != null ? $" {Name}" : "") +
                    ". It has already been settled."
                );
            }

            State = PromiseState.Rejected;
            RejectionException = exception;
            InvokeProgress(1);
            InvokeRejection(exception);
        }

        public void Progress(float amount)
        {
            if (State.IsSettled())
            {
                throw new PromiseStateException(
                    message:
                    "Cannot progress promise" +
                    (Name != null ? $" {Name}" : "") +
                    ". It has already been settled."
                );
            }

            InvokeProgress(amount);
        }

        public void Done()
        {
            if (State is PromiseState.Fulfilled) return;

            Catch(ThrowUnhandledException);
        }
        
        static void ThrowUnhandledException(Exception exception)
        {
            throw exception;
        }
        #endregion
        
        #region Start
        public static Promise Start(Action<Promise> callback, Action<float> onProgress = null)
        {
            Promise p = new Promise();
            
            if (onProgress != null) p.AddProgressHandler(onProgress);
            callback?.Invoke(p);

            return p;
        }
        #endregion

        #region Then
        public Promise Then(Action onFulfill)
            => Then(onFulfillSync: onFulfill);
        
        public Promise Then(Action<Promise> onFulfill, Action<float> onProgress = null)
            => Then(onFulfillAsync: onFulfill, onProgress: onProgress);
        
        public Promise Catch(Action<Exception, Promise> onReject, Action<float> onProgress = null)
            => Then(onRejectAsync: onReject, onProgress: onProgress);
        
        public Promise Catch(Action<Exception> onReject)
            => Then(onRejectSync: onReject);

        public Promise Then(Action onFulfill, Action<Exception> onReject)
            => Then(onFulfillSync: onFulfill, onRejectSync: onReject);

        public Promise Then(Action onFulfill, Action<Exception, Promise> onReject, Action<float> onProgress = null)
            => Then(onFulfillSync: onFulfill, onRejectAsync: onReject, onProgress: onProgress);

        public Promise Then(Action<Promise> onFulfill, Action<Exception> onReject, Action<float> onProgress = null)
            => Then(onFulfillAsync: onFulfill, onRejectSync: onReject, onProgress: onProgress);

        public Promise Then(Action<Promise> onFulfill, Action<Exception, Promise> onReject, Action<float> onProgress = null)
            => Then(onFulfillAsync: onFulfill, onRejectAsync: onReject, onProgress: onProgress);

        // ReSharper disable once MethodOverloadWithOptionalParameter
        Promise Then(
            Action onFulfillSync = null, Action<Promise> onFulfillAsync = null,
            Action<Exception> onRejectSync = null, Action<Exception, Promise> onRejectAsync = null,
            Action<float> onProgress = null
        )
        {
            Promise child = new(Name);

            // promise has already been settled, perform callbacks immediately
            if (State is PromiseState.Fulfilled)
            {
                onFulfillSync?.Invoke();
                onFulfillAsync?.Invoke(child);
            }
            else if (State is PromiseState.Rejected)
            {
                onRejectSync?.Invoke(RejectionException);
                onRejectAsync?.Invoke(RejectionException, child);
            }
            // promise is not settled yet, save callbacks for when it
            else
            {
                // save callbacks
                if (onFulfillSync != null) AddFulfillHandler(onFulfillSync);
                if (onRejectSync != null) AddRejectHandler(onRejectSync);

                if (onFulfillAsync != null) AddFulfillHandler(() => onFulfillAsync(child));
                if (onRejectAsync != null) AddRejectHandler(e => onRejectAsync(e, child));
                
                if (onProgress != null) child.AddProgressHandler(onProgress);
            }

            // chain promise
            if (onFulfillSync != null) AddFulfillHandler(child.Fulfill);
            if (onRejectSync != null) AddRejectHandler(child.Reject);

            return child;
        }
        #endregion

        #region All
        public static Promise All(params Action<Promise>[] callbacks) => All((IEnumerable<Action<Promise>>) callbacks);
        public static Promise All(IEnumerable<Action<Promise>> callbacks)
        {
            Promise root = new Promise();
            Promise group = new Promise();
            root.Fulfill();

            All(root, group, callbacks);
            return group;
        }
        
        public Promise ThenAll(params Action<Promise>[] callbacks) => ThenAll((IEnumerable<Action<Promise>>) callbacks);
        public Promise ThenAll(IEnumerable<Action<Promise>> callbacks)
        {
            Promise group = new();
            All(this, group, callbacks);

            return group;
        }

        static void All(Promise root, Promise group, IEnumerable<Action<Promise>> callbacks)
        {
            Action<Promise>[] callbackArray = callbacks as Action<Promise>[] ?? callbacks.ToArray();
            int callbacksLeft = callbackArray.Length;
            float callbackAmount = callbacksLeft;

            // short-circuit, there are no promises to perform in sequence
            if (callbackAmount == 0)
            {
                group.Fulfill();
                return;
            }

            foreach (Action<Promise> callback in callbackArray)
            {
                Promise promise = root.Then(callback);
                promise.AddFulfillHandler(() =>
                {
                    callbacksLeft--;

                    float fraction = (callbackAmount - callbacksLeft) / callbackAmount;
                    group.Progress(fraction);
                    
                    if (callbacksLeft <= 0)
                    {
                        group.Fulfill();
                    }
                });
            }
        }
        #endregion
        
        #region Race
        public static Promise Race(params Action<Promise>[] callbacks) => Race((IEnumerable<Action<Promise>>) callbacks);
        public static Promise Race(IEnumerable<Action<Promise>> callbacks)
        {
            Promise root = new Promise();
            Promise group = new Promise();
            root.Fulfill();
            
            Race(root, group, callbacks);
            return group;
        }
        
        public Promise ThenRace(params Action<Promise>[] callbacks) => ThenRace((IEnumerable<Action<Promise>>) callbacks);
        public Promise ThenRace(IEnumerable<Action<Promise>> callbacks)
        {
            Promise group = new Promise();
            
            Race(this, group, callbacks);
            return group;
        }

        static void Race(Promise root, Promise group, IEnumerable<Action<Promise>> callbacks)
        {
            Action<Promise>[] callbacksArray = callbacks as Action<Promise>[] ?? callbacks.ToArray();
            
            // short-circuit, there are no promises to wait for
            if (callbacksArray.Length == 0)
            {
                group.Fulfill();
                return;
            }
            
            // start callback after the root is fulfilled
            foreach (Action<Promise> callback in callbacksArray)
            {
                Promise promise = root.Then(callback);
                
                // try to complete race
                promise.AddFulfillHandler(() =>
                {
                    if (group.State is PromiseState.Pending)
                    {
                        group.Fulfill();
                    }
                });
            }
        }
        #endregion
        
        #region Sequence
        public static Promise Sequence(params Action<Promise>[] callbacks) => Sequence((IEnumerable<Action<Promise>>) callbacks);
        public static Promise Sequence(IEnumerable<Action<Promise>> callbacks)
        {
            Promise root = new Promise();
            Promise group = new Promise();
            
            root.Fulfill();
            Sequence(root, group, callbacks);
            
            return group;
        }

        public Promise ThenSequence(params Action<Promise>[] callbacks) => ThenSequence((IEnumerable<Action<Promise>>) callbacks);
        public Promise ThenSequence(IEnumerable<Action<Promise>> callbacks)
        {
            return Then(promise =>
            {
                Sequence(this, promise, callbacks);
            });
        }

        static void Sequence(Promise root, Promise group, IEnumerable<Action<Promise>> callbacks)
        {
            Action<Promise>[] callbackArray = callbacks as Action<Promise>[] ?? callbacks.ToArray();

            // short-circuit, there are no promises to perform in sequence
            if (callbackArray.Length == 0)
            {
                group.Fulfill();
                return;
            }

            Promise prevPromise = root;
            foreach (Action<Promise> callback in callbackArray)
            {
                prevPromise = prevPromise.Then(promise =>
                {
                    callback(promise);
                });
            }
            
            prevPromise.AddFulfillHandler(group.Fulfill);
        }
        #endregion
    }
    
    public enum PromiseState
    {
        Pending = 0, Fulfilled = 1, Rejected = 2,
    }

    public static class PromiseStateExtensions
    {
        public static bool IsSettled(this PromiseState state)
        {
            return state is PromiseState.Fulfilled or PromiseState.Rejected;
        }
    }

    public class PromiseStateException : Exception
    {
        public PromiseStateException(string message) : base(message) { }
    }
}
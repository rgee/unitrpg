using System;
using strange.extensions.signal.impl;

namespace Assets.Contexts {
    public static class StrangeUtils {
        /// <summary>
        /// These listener removal methods get around the bug in StrangeIoC that
        /// prevents detachment of `once` listeners.
        /// </summary>
        public static void RemoveOnceListener(Signal signal, Action action) {
            signal.OnceListener -= action;
        } 

        public static void RemoveOnceListener<T>(Signal<T> signal, Action<T> action) {
            signal.OnceListener -= action;
        }

        public static void Bubble(Signal source, Signal sink) {
            source.AddListener(() => sink.Dispatch());
        }
    }
}
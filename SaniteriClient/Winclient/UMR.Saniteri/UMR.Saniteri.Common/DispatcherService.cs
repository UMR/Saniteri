using System;
using System.Windows;
using System.Windows.Threading;

namespace UMR.Saniteri.Common
{
    public interface IDispatcherService
    {
        void beginDispatch(Action action);
        void dispatch(Action action);
        void beginDispatch<T>(Action<T> action, T arg);
        Dispatcher dispatcher { get; }
    }

    public class DispatcherService : IDispatcherService
    {
        private static IDispatcherService _dispatcherService;
        public static IDispatcherService dispatcherService
        {
            get
            {
                if (_dispatcherService == null) _dispatcherService = new DispatcherService();
                return _dispatcherService;
            }
            set { _dispatcherService = value; }
        }

        private DispatcherService()
        {
            if (Application.Current != null) this.dispatcher = Application.Current.Dispatcher;
            else this.dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void beginDispatch(Action action)
        {
            this.dispatcher.BeginInvoke(action);
        }

        public void dispatch(Action action)
        {
            this.dispatcher.Invoke(action);
        }

        public void beginDispatch<T>(Action<T> action, T arg)
        {
            this.dispatcher.BeginInvoke(action, arg);
        }

        public Dispatcher dispatcher { get; private set; }
    }
}
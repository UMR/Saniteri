using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using UMR.Saniteri.Common;
using UMR.Saniteri.Data;

namespace UMR.Saniteri
{
    public static class SingleInstanceCreator
    {
        internal static void make(string eventName, Application app)
        {
            bool createNew;
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventName, out createNew);
            if (createNew)
            {
                ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, waitTimerCallback, app, Timeout.Infinite, false);
                eventWaitHandle.Close();
            }
            else
            {
                eventWaitHandle.Set();
                Environment.Exit(0);
            }
        }

        private static void waitTimerCallback(Object state, Boolean timedOut)
        {
            DispatcherService.dispatcherService.dispatch(() => Application.Current.MainWindow.Activate());
        }
    }
}

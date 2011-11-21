using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UMR.Saniteri.CustomControls
{

    public class Clock : Control, IDisposable
    {
        private DispatcherTimer timer;

        static Clock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Clock), new FrameworkPropertyMetadata(typeof(Clock)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.updateDateTime();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(1000 - DateTime.Now.Millisecond);
            this.timer.Tick += this.timer_Tick;
            this.timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.updateDateTime();
            this.timer.Interval = TimeSpan.FromMilliseconds(1000 - DateTime.Now.Millisecond);
            this.timer.Start();
        }

        private void updateDateTime()
        {
            this.dateTime = System.DateTime.Now;
        }

        #region DateTime property

        public DateTime dateTime
        {
            get { return (DateTime)GetValue(dateTimeProperty); }
            private set { SetValue(dateTimeProperty, value); }
        }

        public static DependencyProperty dateTimeProperty = DependencyProperty.Register("dateTime", typeof(DateTime), typeof(Clock), new PropertyMetadata(DateTime.Now, new PropertyChangedCallback(onDateTimeInvalidated)));

        public static readonly RoutedEvent dateTimeChangedEvent = EventManager.RegisterRoutedEvent("dateTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(Clock));

        protected virtual void onDateTimeChanged(DateTime oldValue, DateTime newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime> args = new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue);
            args.RoutedEvent = Clock.dateTimeChangedEvent;
            RaiseEvent(args);
        }

        private static void onDateTimeInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Clock)d).onDateTimeChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        #endregion

        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Tick -= this.timer_Tick;
                this.timer.Stop();
                this.timer = null;
            }
        }
    }
}

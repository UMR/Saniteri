using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace UMR.Saniteri.CustomControls
{
    public class SearchBox : TextBox
    {
        public static DependencyProperty labelTextProperty =
            DependencyProperty.Register("labelText", typeof(string), typeof(SearchBox));

        public string labelText
        {
            get { return (string)GetValue(labelTextProperty); }
            set { SetValue(labelTextProperty, value); }
        }

        public static DependencyProperty labelTextColorProperty =
            DependencyProperty.Register("labelTextColor", typeof(Brush), typeof(SearchBox));

        public Brush labelTextColor
        {
            get { return (Brush)GetValue(labelTextColorProperty); }
            set { SetValue(labelTextColorProperty, value); }
        }

        public static DependencyProperty searchModeProperty =
            DependencyProperty.Register("searchMode", typeof(searchMode), typeof(SearchBox), new PropertyMetadata(searchMode.instant));

        public searchMode searchMode
        {
            get { return (searchMode)GetValue(searchModeProperty); }
            set { SetValue(searchModeProperty, value); }
        }

        private static DependencyPropertyKey hasTextPropertyKey =
            DependencyProperty.RegisterReadOnly("hasText", typeof(bool), typeof(SearchBox), new PropertyMetadata(false));

        public bool hasText
        {
            get { return (bool)GetValue(hasTextProperty); }
            private set { SetValue(hasTextPropertyKey, value); }
        }

        public static DependencyProperty hasTextProperty = hasTextPropertyKey.DependencyProperty;

        public Duration searchEventTimeDelay
        {
            get { return (Duration)GetValue(searchEventTimeDelayProperty); }
            set { SetValue(searchEventTimeDelayProperty, value); }
        }

        private static DependencyPropertyKey IsMouseLeftButtonDownPropertyKey =
            DependencyProperty.RegisterReadOnly("IsMouseLeftButtonDown", typeof(bool), typeof(SearchBox), new PropertyMetadata());


        public static DependencyProperty IsMouseLeftButtonDownProperty = IsMouseLeftButtonDownPropertyKey.DependencyProperty;

        public bool IsMouseLeftButtonDown
        {
            get { return (bool)GetValue(IsMouseLeftButtonDownProperty); }
            private set { SetValue(IsMouseLeftButtonDownPropertyKey, value); }
        }

        public static DependencyProperty searchEventTimeDelayProperty =
            DependencyProperty.Register("searchEventTimeDelay", typeof(Duration), typeof(SearchBox),
                new FrameworkPropertyMetadata(new Duration(new TimeSpan(0, 0, 0, 0, 500)), new PropertyChangedCallback(OnSearchEventTimeDelayChanged)));

        public static DependencyProperty commandProperty =
           DependencyProperty.Register("command", typeof(ICommand), typeof(SearchBox));

        public ICommand command
        {
            get { return (ICommand)GetValue(commandProperty); }
            set { SetValue(commandProperty, value); }
        }

        public static readonly RoutedEvent searchEvent =
            EventManager.RegisterRoutedEvent("search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));

        public event RoutedEventHandler search
        {
            add { AddHandler(searchEvent, value); }
            remove { RemoveHandler(searchEvent, value); }
        }

        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }

        private DispatcherTimer searchEventDelayTimer;

        public SearchBox()
            : base()
        {
            this.searchEventDelayTimer = new DispatcherTimer();
            this.searchEventDelayTimer.Interval = searchEventTimeDelay.TimeSpan;
            this.searchEventDelayTimer.Tick += new EventHandler(OnSeachEventDelayTimerTick);
        }

        private void OnSeachEventDelayTimerTick(object o, EventArgs e)
        {
            this.searchEventDelayTimer.Stop();
            this.raiseSearchEvent();
        }

        private static void OnSearchEventTimeDelayChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SearchBox stb = sender as SearchBox;
            if (stb != null)
            {
                stb.searchEventDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                stb.searchEventDelayTimer.Stop();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.hasText = this.Text.Length != 0;
            if (this.searchMode == searchMode.instant)
            {
                this.searchEventDelayTimer.Stop();
                this.searchEventDelayTimer.Start();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Border iconBorder = GetTemplateChild("PART_SearchIconBorder") as Border;
            if (iconBorder != null)
            {
                iconBorder.MouseLeftButtonDown += this.iconBorder_MouseLeftButtonDown;
                iconBorder.MouseLeftButtonUp += this.iconBorder_MouseLeftButtonUp;
                iconBorder.MouseLeave += this.iconBorder_MouseLeave;
            }
        }

        private void iconBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsMouseLeftButtonDown = true;
        }

        private void iconBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsMouseLeftButtonDown) return;
            if (this.hasText && searchMode == searchMode.instant)
            {
                this.Text = "";
            }
            if (this.hasText && searchMode == searchMode.delayed)
            {
                this.raiseSearchEvent();
            }
            this.IsMouseLeftButtonDown = false;
        }

        private void iconBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IsMouseLeftButtonDown = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && searchMode == searchMode.instant)
                this.Text = "";
            else if ((e.Key == Key.Return || e.Key == Key.Enter) && this.searchMode == searchMode.delayed)
                this.raiseSearchEvent();
            else base.OnKeyDown(e);
        }

        private void raiseSearchEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(searchEvent);
            this.RaiseEvent(args);
            if (this.command != null) this.command.Execute(this.Text);
        }
    }

    public enum searchMode { instant, delayed, }
}

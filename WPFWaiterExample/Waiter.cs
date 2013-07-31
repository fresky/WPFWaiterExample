using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPFWaiterExample
{
    internal abstract class Waiter
    {
        protected int m_FinishedJob;
        private Stopwatch m_StopWatch;
        private DispatcherTimer m_Timer;

        private readonly Control m_SpinWaiter;
        protected readonly ProgressBar m_Progressbar;
        private readonly TextBox m_ElaspeTimeTextBox;
        private readonly Button m_Start;
        private readonly Button m_Cancel;

        protected CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();

        internal Waiter(Control spin, ProgressBar progressBar, TextBox elapseTimeTextBox, Button start, Button cancel)
        {
            m_SpinWaiter = spin;
            m_Progressbar = progressBar;
            m_ElaspeTimeTextBox = elapseTimeTextBox;
            m_Start = start;
            m_Cancel = cancel;
        }

        internal abstract void Start();

        internal void Cancel()
        {
            m_CancellationTokenSource.Cancel();
        }

        protected void startWaiting()
        {
            m_Cancel.IsEnabled = true;
            m_Start.IsEnabled = false;
            m_SpinWaiter.Visibility = Visibility.Visible;

            m_FinishedJob = 0;
            m_Progressbar.Value = m_FinishedJob;

            m_Timer = new DispatcherTimer();
            m_StopWatch = new Stopwatch();

            m_Timer.Tick += updateElapseTime;
            m_Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            m_StopWatch.Start();
            m_Timer.Start();
        }

        protected void stopWaiting()
        {
            m_Cancel.IsEnabled = false;
            m_Start.IsEnabled = true;

            m_Timer.Tick -= updateElapseTime;
            updateElapseTime(null, null);

            m_Timer.Stop();
            m_StopWatch.Stop();

            m_SpinWaiter.Visibility = Visibility.Collapsed;
        }

        private void updateElapseTime(object sender, EventArgs e)
        {
            m_ElaspeTimeTextBox.Text = m_StopWatch.Elapsed.TotalSeconds.ToString("F2");
        }
    }
}
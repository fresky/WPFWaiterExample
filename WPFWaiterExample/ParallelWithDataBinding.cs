using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WPFWaiterExample
{
    internal class ParallelWithDataBinding :  INotifyPropertyChanged
    {
        private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();

        private Stopwatch m_StopWatch;
        private DispatcherTimer m_Timer;

        private int m_FinishedJob;
        private bool m_StartButtonEnabled;
        private bool m_CancelButtonEnabled;
        private Visibility m_SpinVisibility;
        private string m_ElapseTime;

        public ParallelWithDataBinding()
        {
            SpinVisibility = Visibility.Collapsed;
            StartButtonEnabled = true;
            CancelButtonEnabled = false;
        }

        public bool StartButtonEnabled
        {
            get { return m_StartButtonEnabled; }
            private set
            {
                m_StartButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool CancelButtonEnabled
        {
            get { return m_CancelButtonEnabled; }
            private set
            {
                m_CancelButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public int FinishedJob
        {
            get { return m_FinishedJob; }
            private set
            {
                m_FinishedJob = value;
                OnPropertyChanged();
            }
        }

        public Visibility SpinVisibility
        {
            get { return m_SpinVisibility; }
            private set
            {
                m_SpinVisibility = value;
                OnPropertyChanged();
            }
        }

        public string ElapseTime
        {
            get { return m_ElapseTime; }
            private set
            {
                m_ElapseTime = value;
                OnPropertyChanged();
            }
        }

        public void Start()
        {
            startWaiting();
            
            Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= Job.JobNumber; i++)
                {
                    Task.Factory.StartNew(Job.TimeConsumingJob, m_CancellationTokenSource.Token).ContinueWith(antecedent =>
                        {
                            Interlocked.Increment(ref m_FinishedJob);
                            FinishedJob = m_FinishedJob;
                        },m_CancellationTokenSource.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Default);
                }
            }).ContinueWith(t =>
            {
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    m_CancellationTokenSource = new CancellationTokenSource();
                }

                stopWaiting();
            });
        }

        public void Cancel()
        {
            m_CancellationTokenSource.Cancel();
        }

        private void startWaiting()
        {
            FinishedJob = 0;
            StartButtonEnabled = false;
            CancelButtonEnabled = true;
            SpinVisibility = Visibility.Visible;

            m_Timer = new DispatcherTimer();
            m_Timer.Tick += updateElapseTime;
            m_Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            m_StopWatch = new Stopwatch();
            m_StopWatch.Start();
            m_Timer.Start();
        }

        private void stopWaiting()
        {
            StartButtonEnabled = true;
            CancelButtonEnabled = false;
            SpinVisibility = Visibility.Collapsed;

            m_Timer.Tick -= updateElapseTime;
            m_Timer.Stop();
            m_StopWatch.Stop();
        }

        private void updateElapseTime(object sender, EventArgs e)
        {
            ElapseTime = m_StopWatch.Elapsed.TotalSeconds.ToString("F2");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
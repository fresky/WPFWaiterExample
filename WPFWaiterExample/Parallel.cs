using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFWaiterExample
{
    internal class Parallel: Waiter
    {
        public Parallel(Control spin, ProgressBar progressBar, TextBox elapseTimeTextBox, Button start, Button cancel) : base(spin, progressBar, elapseTimeTextBox, start, cancel)
        {
        }

        internal override async void Start()
        {
            startWaiting();

            List<Task> taskList = new List<Task>();
            for (int i = 1; i <= Job.JobNumber; i++)
            {
                taskList.Add(
                        Task.Factory.StartNew(Job.TimeConsumingJob).ContinueWith(t =>
                            {
                                m_FinishedJob++;
                                m_Progressbar.Value = m_FinishedJob;
                            },
                        m_CancellationTokenSource.Token,
                        TaskContinuationOptions.None,
                        TaskScheduler.FromCurrentSynchronizationContext())
                        );
            }

            try
            {
                await Task.WhenAll(taskList);
            }
            catch (OperationCanceledException)
            {
                m_CancellationTokenSource = new CancellationTokenSource();
            }

            stopWaiting();
        }
    }
}
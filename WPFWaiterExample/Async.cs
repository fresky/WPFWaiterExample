using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFWaiterExample
{
    internal class Async:Waiter
    {
        public Async(Control spin, ProgressBar progressBar, TextBox elapseTimeTextBox, Button start, Button cancel) : base(spin, progressBar, elapseTimeTextBox, start, cancel)
        {
        }

        internal override async void Start()
        {
            startWaiting();

            try
            {
                for (int i = 1; i <= Job.JobNumber; i++)
                {
                    await Task.Factory.StartNew(Job.TimeConsumingJob, m_CancellationTokenSource.Token);
                    m_FinishedJob++;
                    m_Progressbar.Value = m_FinishedJob;
                }
            }
            catch (OperationCanceledException)
            {
                m_CancellationTokenSource = new CancellationTokenSource();
            }
            
            stopWaiting();
        }
    }
}
using System.Windows.Controls;

namespace WPFWaiterExample
{
    internal class Sync:Waiter
    {
        public Sync(Control spin, ProgressBar progressBar, TextBox elapseTimeTextBox, Button start, Button cancel) : base(spin, progressBar, elapseTimeTextBox, start, cancel)
        {
        }

        internal override void Start()
        {
            startWaiting();
            for (int i = 1; i <= Job.JobNumber; i++)
            {
                Job.TimeConsumingJob();
                m_FinishedJob++;
                m_Progressbar.Value = m_FinishedJob;
            }
            stopWaiting();
        }

    }
}
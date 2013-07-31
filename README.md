WPFWaiterExample
================

This project is a WPF progress bar, spin waiter and elapse time example.


## Usage

Please refer to the below screen shot.

![screenshot](https://raw.github.com/fresky/WPFWaiterExample/master/screenshot.png "WPFWaiterExample")

1. F5 to run the project.
2. Click the **"Start"** button to start the job.
3. Click the **"Cancel"** button to cancel the job.



There are 4 implementations in this example.  
  
1. Sync version.  
The progress bar, spin waiter and elapse time will not update since all work is done by the UI thread.    
```C#
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
```

2. Async version.  
Using ``await`` and ``async`` keyword provided by .NET 4.5 to implement the async version. We can see the progress bar, spin waiter and elapse time will update during the job. And user can cancel the operation.
```C#
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
```

3. Parallel version.  
Make the job running in parallel.
```C#
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
```

4. Parallel with data binding version.

## Requirements

Require TPL support in [.NET Framework 4.5](http://msdn.microsoft.com/library/vstudio/5a4x27ek).

## Credits
WPFWaiterExample used the [spinner provided by Drew Noakes in Stackoverflow](http://stackoverflow.com/a/1492141/304115).


## License

WPFWaiterExample is released under the MIT License. See the bundled LICENSE file for details.

## Chang Log

1. 07/31/2013	initial version
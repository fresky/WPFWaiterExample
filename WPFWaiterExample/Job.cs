using System;
using System.Threading;

static internal class Job
{
    public static void TimeConsumingJob()
    {
        Thread.Sleep(300);
    }

    public const int JobNumber = 100;
}
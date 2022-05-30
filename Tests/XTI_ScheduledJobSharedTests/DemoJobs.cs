namespace XTI_ScheduledJobSharedTests;

public static class DemoJobs
{
    public static class DoSomething
    {
        public static readonly JobKey JobKey = new JobKey(nameof(DoSomething));
        public static readonly JobTaskKey Task01 = new JobTaskKey(nameof(Task01));
        public static readonly JobTaskKey Task02 = new JobTaskKey(nameof(Task02));
        public static readonly JobTaskKey TaskItem01 = new JobTaskKey(nameof(TaskItem01));
        public static readonly JobTaskKey TaskItem02 = new JobTaskKey(nameof(TaskItem02));
    }
}

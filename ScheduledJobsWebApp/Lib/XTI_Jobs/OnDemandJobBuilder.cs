using XTI_Core;

namespace XTI_Jobs;

public sealed class OnDemandJobBuilder
{
    private readonly IJobDb db;
    private JobKey? jobKey;
    private IJobActionFactory? jobActionFactory;
    private string[]? data;
    private TimeSpan deleteAfter = TimeSpan.FromDays(365);

    public OnDemandJobBuilder(IJobDb db)
    {
        this.db = db;
    }

    public OnDemandJobBuilder1 ForJob(JobKey jobKey)
    {
        this.jobKey = jobKey;
        return new OnDemandJobBuilder1(this);
    }

    internal void WithData(string[] data)
    {
        this.data = data;
    }

    internal void UseJobActionFactory(IJobActionFactory jobActionFactory)
    {
        this.jobActionFactory = jobActionFactory;
    }

    internal void DeleteAfter(TimeSpan deleteAfter)
    {
        this.deleteAfter = deleteAfter;
    }

    internal OnDemandJob Build() =>
        new OnDemandJob
        (
            db,
            jobKey ?? throw new ArgumentNullException(nameof(jobKey)),
            jobActionFactory ?? throw new ArgumentNullException(nameof(jobActionFactory)),
            data ?? throw new ArgumentNullException(nameof(data)),
            deleteAfter
        );
}

public sealed class OnDemandJobBuilder1
{
    private readonly OnDemandJobBuilder builder;

    internal OnDemandJobBuilder1(OnDemandJobBuilder builder)
    {
        this.builder = builder;
    }

    public OnDemandJobBuilder2 WithData(params object[] data) =>
        WithData(d => XtiSerializer.Serialize(d), data);

    public OnDemandJobBuilder2 WithData(Func<object, string> serialize, params object[] data)
    {
        var dataSerializations = data.Select(d => serialize(d)).ToArray();
        return WithSerializedData(dataSerializations);
    }

    public OnDemandJobBuilder2 WithSerializedData(string[] data)
    {
        builder.WithData(data);
        return new OnDemandJobBuilder2(builder);
    }
}

public sealed class OnDemandJobBuilder2
{
    private readonly OnDemandJobBuilder builder;

    internal OnDemandJobBuilder2(OnDemandJobBuilder builder)
    {
        this.builder = builder;
    }

    public OnDemandJobBuilderFinal UseJobActionFactory(IJobActionFactory jobActionFactory)
    {
        builder.UseJobActionFactory(jobActionFactory);
        return new OnDemandJobBuilderFinal(builder);
    }
}

public sealed class OnDemandJobBuilderFinal
{
    private readonly OnDemandJobBuilder builder;

    internal OnDemandJobBuilderFinal(OnDemandJobBuilder builder)
    {
        this.builder = builder;
    }

    public OnDemandJobBuilderFinal DeleteAfter(TimeSpan deleteAfter)
    {
        builder.DeleteAfter(deleteAfter);
        return this;
    }

    public OnDemandJob Build() => builder.Build();
}
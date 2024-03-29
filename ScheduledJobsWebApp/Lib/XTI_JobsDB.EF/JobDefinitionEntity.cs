﻿namespace XTI_JobsDB.EF;

public sealed class JobDefinitionEntity
{
    public int ID { get; set; }
    public string JobKey { get; set; } = "";
    public string DisplayText { get; set; } = "";
    public TimeSpan Timeout { get; set; } = TimeSpan.Zero;
    public TimeSpan DeleteAfter { get; set; } = TimeSpan.Zero;
}

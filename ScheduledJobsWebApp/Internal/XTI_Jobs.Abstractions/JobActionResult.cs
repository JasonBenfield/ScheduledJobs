namespace XTI_Jobs.Abstractions;

public sealed record JobActionResult(TriggeredJobTaskModel CompletedTask, NextTaskModel[] NextTasks);
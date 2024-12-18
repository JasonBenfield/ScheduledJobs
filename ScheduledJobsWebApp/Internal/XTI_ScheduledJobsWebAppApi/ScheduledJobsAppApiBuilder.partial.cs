namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApiBuilder
{
    partial void Configure()
    {
        source.ConfigureTemplate
        (
            template =>
            {
                template.ExcludeValueTemplates
                (
                    (temp, generators) =>
                    {
                        if (generators == ApiCodeGenerators.Dotnet)
                        {
                            var ns = temp.DataType.Namespace ?? "";
                            return ns.StartsWith("XTI_Jobs.Abstractions") || ns.StartsWith("XTI_Hub.Abstractions");
                        }
                        return false;
                    }
                );
            }
        );
    }
}

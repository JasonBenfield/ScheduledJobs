namespace XTI_JobsDB.EF;

public sealed class HierarchicalTriggeredJobTaskEntity
{
    public int ID { get; set; }
    public int ParentTaskID { get; set; }
    public int ChildTaskID { get; set; }
}

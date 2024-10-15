namespace TasksProject.Models
{
    public class Tasks
    {
     
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsCompleted { get; set; }
            public int UserId { get; set; }
            public Users User { get; set; }
            public Status TaskStatus { get; set; }
    }
    public enum Status { 
        Assigned = 0,
        InProgress = 1,
        Closed = 2 ,
        Started = 3 ,
        Resolved = 4
    }

}

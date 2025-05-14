namespace Application.Boards.GetBoardTask;

//internal sealed class GetBoardTaskQuery
public sealed class BoardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Order { get; set; }
    public bool IsArchive { get; set; }
    public List<TaskResponse> TaskResponses { get; set; }
}

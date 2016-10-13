namespace Security.Interfaces.Model
{
    public interface IGrant
    {
        int IdSecObject { get; }
        int IdRole { get; }
        int IdAccessType { get; }
        IRole Role { get; set; }
        IAccessType AccessType { get; set; }
        ISecObject SecObject { get; set; }
    }
}
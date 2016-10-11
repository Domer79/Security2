namespace Security.Interfaces.Model
{
    public interface IGrant
    {
        int IdSecObject { get; }
        int IdRole { get; }
        int IdAccessType { get; }
        IRole Role { get; }
        IAccessType AccessType { get; }
        ISecObject SecObject { get; }
    }
}
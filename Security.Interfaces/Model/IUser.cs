using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface IUser
    {
        int IdMember { get; }
        string Login { get; }
        byte[] Password { get; }
        IList<IGroup> Groups { get; }
    }
}
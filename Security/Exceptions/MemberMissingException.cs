using System;

namespace Security.Exceptions
{
    public class MemberMissingException : Exception
    {
        public MemberMissingException(string memberName)
            : base($"This member \"{memberName}\" is missing!")
        {
            
        }
    }
}
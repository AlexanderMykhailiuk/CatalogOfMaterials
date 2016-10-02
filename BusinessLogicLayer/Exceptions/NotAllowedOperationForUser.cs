using System;

namespace BusinessLogicLayer.Exceptions
{
    public class NotAllowedOperationForUser : Exception
    {
        public NotAllowedOperationForUser(string UserName)
            : base(string.Format("User {0} can't make this operation")) { }
    }
}

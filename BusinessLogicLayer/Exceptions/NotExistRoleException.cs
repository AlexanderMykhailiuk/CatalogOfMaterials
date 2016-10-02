using System;

namespace BusinessLogicLayer.Exceptions
{
    class NotExistRoleException : Exception
    {
        public NotExistRoleException(string RoleName) :
            base(string.Format("{0} not exist",RoleName))
        { }
    }
}

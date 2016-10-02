using System;

namespace BusinessLogicLayer.Exceptions
{
    public class NotExistUserException : Exception
    {
        public NotExistUserException(string username) 
            : base(string.Format("User with username={0} not exist", username)) { }
    }
}

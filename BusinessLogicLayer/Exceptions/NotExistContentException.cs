using System;

namespace BusinessLogicLayer.Exceptions
{
    public class NotExistContentException : Exception
    {
        public NotExistContentException(int id) 
            : base(string.Format("Content with id={0} not exist",id)) { } 
    }
}

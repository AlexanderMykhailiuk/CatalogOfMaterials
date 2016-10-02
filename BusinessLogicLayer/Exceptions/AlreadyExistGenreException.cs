using System;

namespace BusinessLogicLayer.Exceptions
{
    public class AlreadyExistGenreException : Exception
    {
        public AlreadyExistGenreException(string GenreName) :
            base(string.Format("{0} allready exist", GenreName))
        { }
    }
}

using System;

namespace BusinessLogicLayer.Exceptions
{
    public class GenreContainContentsException : Exception
    {
        public GenreContainContentsException(string GenreName) :
            base(string.Format("{0} contain contents - firstly remove all contents in this genre", GenreName))
        { }
    }
}

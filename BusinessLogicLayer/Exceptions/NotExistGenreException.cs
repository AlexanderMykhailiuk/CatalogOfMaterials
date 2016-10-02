using System;

namespace BusinessLogicLayer.Exceptions
{
    public class NotExistGenreException : Exception
    {
        public NotExistGenreException(int GenreId) :
            base(string.Format("Genre {0} not exist in system. Ask admin or moderator for adding this one.",GenreId))
        {

        } 
    }
}

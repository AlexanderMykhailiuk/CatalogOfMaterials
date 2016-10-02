using System;
using System.Collections.Generic;
using BusinessLogicLayer.DataTransferObjects;

namespace BusinessLogicLayer.Interfaces
{
    public interface IContentService : IDisposable
    {
        IEnumerable<GenreDTO> GetAllGenres();

        /// <summary>
        /// Add new content to UOF
        /// </summary>
        /// <param name="UserName">Username of user wich add content</param>
        /// <param name="genre">Genre of new content (search in UOF by id)</param>
        /// <param name="content">New content</param>
        /// <param name="files">Files in new content</param>
        /// <param name="image">Image of new content (can be null)</param>
        /// <exception cref="Exceptions.NotExistGenreException">Thrown when such genre not exist in UOF</exception>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user with UserID not exist</exception>
        void AddContent(string UserName, GenreDTO genre, ContentDTO content, IEnumerable<FileDTO> files, FileDTO image);

        /// <summary>
        /// Get all contents in db
        /// </summary>
        IEnumerable<ContentDTO> GetAllContents();
        
        /// <summary>
        /// Find all about content by id
        /// </summary>
        /// <param name="ID">Id of searching content</param>
        /// <param name="genre">Return genre of searching content</param>
        /// <param name="files">Return files of searching content</param>
        /// <param name="userUploaded">User who uploaded content</param>
        /// <exception cref="Exceptions.NotExistContentException">Thrown when content not found</exception>
        /// <returns>Content with id</returns>
        ContentDTO GetContentByIdWithAllData(int ID, out GenreDTO genre, out IEnumerable<FileDTO> files,out UserDTO userUploaded);

        /// <summary>
        /// Search content, which fiekds contain searching text
        /// </summary>
        /// <param name="searchingText">Text which search in content fields. Can be null - return all contents in genre</param>
        /// <param name="GenreID">Id of genre. Can be null - search in all genres</param>
        /// <returns>Searching contents</returns>
        IEnumerable<ContentDTO> GetSearchingContents(string searchingText, int? GenreID);

        /// <summary>
        /// Search all content of user
        /// </summary>
        /// <param name="UserName">Username of user</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when such user not exist</exception>
        /// <returns>Contents of user</returns>
        IEnumerable<ContentDTO> GetContentsOfUser(string UserName);
        
        /// <summary>
        /// Return file with searching id
        /// </summary>
        /// <param name="id">Id of searching file</param>
        FileDTO GetFileById(int id);

        /// <summary>
        /// Delete content
        /// </summary>
        /// <param name="UserName">Name of user which call operation</param>
        /// <param name="ContentID">Id of deleting content</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't delete choisen content</exception>
        /// <exception cref="Exceptions.NotExistContentException">Thrown when content not exist</exception> 
        void Delete(string UserName, int ContentID);

        /// <summary>
        /// Update Content
        /// </summary>
        /// <param name="UserName">Name of user which call operation</param>
        /// <param name="genre">The new genre of content</param>
        /// <param name="content">Content with updated fields</param>
        /// <param name="files">New files of content. If not null remove all previous files</param>
        /// <param name="image">New image of content. If not null remove previous image</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't delete choisen content</exception>
        /// <exception cref="Exceptions.NotExistGenreException">Thrown when such genre not exist in UOF</exception>
        /// <exception cref="Exceptions.NotExistContentException">Thrown when content not exist</exception> 
        void Update(string UserName, GenreDTO genre, ContentDTO content, IEnumerable<FileDTO> files, FileDTO image);

        /// <summary>
        /// Add new genre
        /// </summary>
        /// <param name="UserName">Name of user who initiate operation</param>
        /// <param name="newGenre">New genre</param>
        /// <exception cref="Exceptions.AlreadyExistGenreException">Thrown when in db already exist genre</exception>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        void AddGenre(string UserName, GenreDTO newGenre);

        /// <summary>
        /// Delete genre, if no exist content in this genre
        /// </summary>
        /// <param name="UserName">Name of user who initiate operation</param>
        /// <param name="Genre"></param>
        /// <exception cref="Exceptions.GenreContainContentsException">Thrown when exist content in this genre</exception>
        /// <exception cref="Exceptions.NotExistGenreException">Thrown when such genre not exist</exception>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        void DeleteGenre(string UserName, GenreDTO genre);

        /// <summary>
        /// Rename genre
        /// </summary>
        /// <param name="UserName">Name of user who initiate operation</param>
        /// <param name="changedGenre">Changed genre</param>
        /// <exception cref="Exceptions.NotExistGenreException">Thrown when genre not exist</exception>
        /// <exception cref="Exceptions.AlreadyExistGenreException">Thrown when genre with such name already exist</exception>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        void RenameGenre(string UserName, GenreDTO changedGenre);
    }
}

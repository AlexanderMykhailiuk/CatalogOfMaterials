using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.DataTransferObjects;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;
using static BusinessLogicLayer.Mapping.MappingConfigs;

namespace BusinessLogicLayer.Services
{
    public class ContentService : IContentService
    {
        readonly IUnitOfWork Database;
        
        public ContentService(IUnitOfWork uow)
        {
            Database = uow;            
        }
        
        public IEnumerable<GenreDTO> GetAllGenres()
        {
            var mapper = GenretoGenreDTOConfig.CreateMapper();
            return mapper.Map<IEnumerable <GenreDTO>>(Database.Genres.GetAll());
        }
        
        public void AddContent(string UserName, GenreDTO genre, ContentDTO content, IEnumerable<FileDTO> files,FileDTO image)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);

            Genre genre_in_db = Database.Genres.Get(genre.GenreID);
            if (genre_in_db == null) throw new Exceptions.NotExistGenreException(genre.GenreID);
            else
            {
                var mapper = DTOToEntetiesConfig.CreateMapper();
                Content content_in_db = mapper.Map<Content>(content);
                // direct linking
                foreach (var file in files)
                {
                    content_in_db.Files.Add(mapper.Map<File>(file));
                }

                // direct linking
                content_in_db.user = user_in_db;

                if (image != null)
                {
                    File image_in_db = mapper.Map<File>(image);
                    Database.Files.Add(image_in_db);
                    Database.Complete();
                    //direct linking
                    image_in_db.content = content_in_db;
                    content_in_db.ImageID = image_in_db.FileID;
                }
                else
                {
                    content_in_db.ImageID = null;
                }
                //direct linking
                genre_in_db.Contents.Add(content_in_db);
                
                Database.Genres.Update(genre_in_db);
                Database.Complete();
            }
        }

        

        public IEnumerable<ContentDTO> GetAllContents()
        {
            var mapper = ContenttoDTOConfig.CreateMapper();
            
            return mapper.Map<IEnumerable<ContentDTO>>(Database.Contents.GetAll());
        }
        
        public ContentDTO GetContentByIdWithAllData(int ID,out GenreDTO genre,out IEnumerable<FileDTO> files,out UserDTO userUploaded)
        {
            var content = Database.Contents.Get(ID);

            if (content != null)
            {
                var mapper = ContenttoDTOConfig.CreateMapper();
                genre = mapper.Map<GenreDTO>(content);
                files = mapper.Map<IEnumerable<FileDTO>>(content);
                userUploaded = mapper.Map<UserDTO>(content);

                return mapper.Map<ContentDTO>(content);
            }
            else
            {
                throw new Exceptions.NotExistContentException(ID);
            }

        }

        public IEnumerable<ContentDTO> GetSearchingContents(string searchingText, int? GenreID)
        {
            var mapper = ContenttoDTOConfig.CreateMapper();

            IEnumerable<ContentDTO> ContenWithSearchingGenre;

            if (GenreID == null) ContenWithSearchingGenre = mapper.Map<IEnumerable<ContentDTO>>(Database.Contents.GetAll());
            else
            {
                int genreID = GenreID ?? -1;
                Genre genre = Database.Genres.Get(genreID);

                if (genre == null) throw new Exceptions.NotExistGenreException(genreID);

                ContenWithSearchingGenre = mapper.Map<IEnumerable<ContentDTO>>(genre.Contents);
            }

            if (searchingText == null) return ContenWithSearchingGenre;
            else
            {
                var querySearchingInName = from cont in ContenWithSearchingGenre
                                           where cont.Name == searchingText
                                           select cont;

                var querySearchingInDescription = from cont in ContenWithSearchingGenre
                                           where cont.Description == searchingText
                                           select cont;

                var querySearchingInAuthor = from cont in ContenWithSearchingGenre
                                           where cont.Author == searchingText
                                           select cont;

                var querySearchingInYear = from cont in ContenWithSearchingGenre
                                           where cont.YearOfCreation.ToString() == searchingText
                                           select cont;


                return querySearchingInName.
                    Union(querySearchingInDescription).
                    Union(querySearchingInAuthor).
                    Union(querySearchingInYear);
            }
        }

        public IEnumerable<ContentDTO> GetContentsOfUser(string UserName)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var mapper = ContenttoDTOConfig.CreateMapper();
                return mapper.Map<IEnumerable<ContentDTO>>(user_in_db.Contents);
            }
        }
        
        public FileDTO GetFileById(int id)
        {
            var mapper = FiletoFileDTOConfig.CreateMapper();

            return mapper.Map<FileDTO>(Database.Files.Get(id));
        }

        public void Delete(string UserName, int ContentID)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);

            Content content_in_db = Database.Contents.Get(ContentID);
            if (content_in_db == null) throw new Exceptions.NotExistContentException(ContentID);
            else
            {
                var roles = Database.UserManager.GetRoles(user_in_db.Id);

                if (roles.Contains("Admin") || roles.Contains("Moderator") || string.Equals(user_in_db.Id, content_in_db.user.Id))
                {
                    // delete files
                    foreach (var file in content_in_db.Files.ToList())
                    {
                        content_in_db.Files.Remove(file);
                    }

                    Database.Complete();
                    
                    Database.Contents.Remove(content_in_db);
                    Database.Complete();
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public void Update(string UserName, GenreDTO genre, ContentDTO content, IEnumerable<FileDTO> files, FileDTO image)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);

            Genre genre_in_db = Database.Genres.Get(genre.GenreID);
            if (genre_in_db == null) throw new Exceptions.NotExistGenreException(genre.GenreID);

            Content content_in_db = Database.Contents.Get(content.ContentID);
            if (content_in_db == null) throw new Exceptions.NotExistContentException(content.ContentID);
            else
            {
                var roles = Database.UserManager.GetRoles(user_in_db.Id);

                if (roles.Contains("Admin") || roles.Contains("Moderator") || string.Equals(user_in_db.Id, content_in_db.user.Id))
                {   
                    // delete old files, if needed
                    foreach (var file in content_in_db.Files.ToList())
                    {
                        if (string.Equals(file.FileType, "image/*"))
                        {
                            if (image != null) content_in_db.Files.Remove(file);
                        }
                        else
                        {
                            if (files != null) content_in_db.Files.Remove(file);
                        }
                    }
                    Database.Complete();

                    User user_owner = content_in_db.user;

                    var mapper = DTOToEntetiesConfig.CreateMapper();

                    {
                        content_in_db.Author = content.Author;
                        content_in_db.Description = content.Description;
                        content_in_db.Name = content.Name;
                        content_in_db.YearOfCreation = content.YearOfCreation;    
                    }
                    
                    // direct linking
                    foreach (var file in files)
                    {
                        content_in_db.Files.Add(mapper.Map<File>(file));
                    }
                    
                    if (image != null)
                    {
                        File image_in_db = mapper.Map<File>(image);
                        Database.Files.Add(image_in_db);
                        Database.Complete();
                        //direct linking
                        image_in_db.content = content_in_db;
                        content_in_db.ImageID = image_in_db.FileID;
                    }

                    Database.Contents.Update(content_in_db);
                    Database.Complete();
                    ;
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public void AddGenre(string UserName, GenreDTO newGenre)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_in_db.Id);

                if (roles.Contains("Admin") || roles.Contains("Moderator"))
                {
                    bool IsExist = Database.Genres.Find(genre => (string.Equals(genre.Name,newGenre.Name))).Count()>0;

                    if (IsExist) throw new Exceptions.AlreadyExistGenreException(newGenre.Name);
                    else
                    {
                        var mapper = GenreDTOtoGenreConfig.CreateMapper();
                        Database.Genres.Add(mapper.Map<Genre>(newGenre));
                        Database.Complete();
                    }
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public void DeleteGenre(string UserName, GenreDTO genre)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_in_db.Id);

                if (roles.Contains("Admin") || roles.Contains("Moderator"))
                {
                    Genre genre_in_db = Database.Genres.Get(genre.GenreID);
                    if (genre_in_db == null) throw new Exceptions.NotExistGenreException(genre.GenreID);
                    else
                    {
                        if (genre_in_db.Contents.Count > 0) throw new Exceptions.GenreContainContentsException(genre_in_db.Name);
                        else
                        {
                            Database.Genres.Remove(genre_in_db);
                            Database.Complete();
                        }
                    }
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public void RenameGenre(string UserName, GenreDTO changedGenre)
        {
            User user_in_db = Database.UserManager.FindByName(UserName);
            if (user_in_db == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_in_db.Id);

                if (roles.Contains("Admin") || roles.Contains("Moderator"))
                {
                    Genre genre_in_db = Database.Genres.Get(changedGenre.GenreID);
                    if (genre_in_db == null) throw new Exceptions.NotExistGenreException(changedGenre.GenreID);
                    else
                    {
                        bool IsExist = Database.Genres.Find(genre => (string.Equals(genre.Name, changedGenre.Name))).Count() > 0;

                        if (IsExist) throw new Exceptions.AlreadyExistGenreException(changedGenre.Name);
                        else
                        {
                            genre_in_db.Name = changedGenre.Name;
                            Database.Genres.Update(genre_in_db);
                            Database.Complete();
                        }
                    }
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Database.Dispose();
                }                
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

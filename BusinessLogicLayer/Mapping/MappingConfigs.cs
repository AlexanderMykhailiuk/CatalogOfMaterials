using System.Collections.Generic;
using AutoMapper;
using DataAccessLayer.Entities;
using BusinessLogicLayer.DataTransferObjects;

namespace BusinessLogicLayer.Mapping
{
    /// <summary>
    /// Configs for automapper. Make public just for unit tests
    /// </summary>
    public static class MappingConfigs
    {
        // configs for users

        public static MapperConfiguration UserDTOtoUserNameConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, string>()
                    .ConvertUsing(x => x.UserName)
                );
            }
        }

        public static MapperConfiguration UserDTOtoEmailConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, string>()
                    .ConvertUsing(x => x.Email)
                );
            }
        }

        public static MapperConfiguration UserDTOtoUserConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()
                    .ConvertUsing(x => new User { Email = x.Email, UserName = x.UserName })
                );
            }
        }

        public static MapperConfiguration UserDTOtoPaswordConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, string>()
                    .ConvertUsing(x => x.Password));
            }
        }

        public static MapperConfiguration UserRoleToUserRoleDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<IList<string>, RoleDTO>().ConvertUsing(x => 
                    {
                        if (x.Contains("Admin")) return new RoleDTO() { Name = "Admin" };
                        if (x.Contains("Moderator")) return new RoleDTO() { Name = "Moderator" };
                        return new RoleDTO() { Name = "User" };
                    });
                    cfg.CreateMap<Role, RoleDTO>().ConvertUsing(x => new RoleDTO() { Name = x.Name });
                    cfg.CreateMap<User, UserDTO>().ConvertUsing(x => new UserDTO()
                        {
                            UserName = x.UserName,
                            Email = x.Email
                        });
                }
                );
            }
        }

        // Configs for content

        public static MapperConfiguration GenretoGenreDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Genre, GenreDTO>());
            }
        }

        public static MapperConfiguration GenreDTOtoGenreConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GenreDTO, Genre>());
            }
        }

        public static MapperConfiguration DTOToEntetiesConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ContentDTO, Content>();
                    cfg.CreateMap<FileDTO, File>();
                });
            }
        }

        public static MapperConfiguration ContenttoDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Content, ContentDTO>();
                    cfg.CreateMap<Content, UserDTO>().ConvertUsing(x => new UserDTO
                    {
                        UserName = x.user.UserName,
                        Email = x.user.Email
                    });
                    cfg.CreateMap<Content, IEnumerable<FileDTO>>().ConvertUsing(x =>
                    {
                        var rez = new List<FileDTO>();

                        foreach (var file in x.Files)
                        {
                            rez.Add(new FileDTO()
                            {
                                FileID = file.FileID,
                                Name = file.Name,
                                FileType = file.FileType,
                                BinaryData = file.BinaryData
                            });
                        }

                        return rez;
                    });
                    cfg.CreateMap<Content, GenreDTO>().ConvertUsing(x => new GenreDTO()
                    {
                        GenreID = x.genre.GenreID,
                        Name = x.genre.Name
                    });
                    cfg.CreateMap<User, UserDTO>();
                }
                );
            }
        }

        public static MapperConfiguration FiletoFileDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<File, FileDTO>()
                );
            }
        }
    }
}

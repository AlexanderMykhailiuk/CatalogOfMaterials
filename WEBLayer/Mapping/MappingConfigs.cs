using System.Collections.Generic;
using System.Web;
using AutoMapper;
using BusinessLogicLayer.DataTransferObjects;
using WEBLayer.Models;

namespace WEBLayer.Mapping
{
    public static class MappingConfigs
    {
        // configs for account controller

        public static MapperConfiguration RegisterModeltoUserDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<RegisterModel, UserDTO>()
                    .ConvertUsing(x => new UserDTO { Email = x.Email, UserName = x.Username, Password = x.Password })
                );
            }
        }

        public static MapperConfiguration LoginModeltoUserDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<LoginModel, UserDTO>()
                    .ConvertUsing(x => new UserDTO { UserName = x.Username, Password = x.Password })
                );
            }
        }

        public static MapperConfiguration UserManageConfig
        {
            get
            {
                return new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<UserDTO, AboutUserModel>();
                    cfg.CreateMap<RoleDTO, RoleModel>();
                    cfg.CreateMap<RoleModel, RoleDTO>();
                });
            }
        }

        // configs for content and home controller

        public static MapperConfiguration ContentDTOtoShortContentModelConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ContentDTO, ShortContentModel>();
                }
                );
            }
        }
        
        public static MapperConfiguration GenreDTOtoGenreModelConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GenreDTO, GenreModel>());
            }
        }

        public static MapperConfiguration ContentUploadModelToDTOConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ContentUploadModel, GenreDTO>().ConvertUsing(x => new GenreDTO() { GenreID = x.GenreID });
                    cfg.CreateMap<ContentUploadModel, ContentDTO>().ConvertUsing(x => new ContentDTO()
                    {
                        Name = x.Name,
                        Author = x.Author,
                        YearOfCreation = x.YearOfCreation,
                        Description = x.Description
                    }
                    );
                    cfg.CreateMap<HttpPostedFileBase, FileDTO>().ConvertUsing(x =>
                    {
                        if (x != null && x.ContentLength > 0)
                        {
                            byte[] binaryData;

                            using (var reader = new System.IO.BinaryReader(x.InputStream))
                            {
                                binaryData = reader.ReadBytes(x.ContentLength);
                            }

                            return new FileDTO()
                            {
                                Name = x.FileName,
                                BinaryData = binaryData,
                                FileType = "image/*"
                            };
                        }
                        else return null;
                    });
                    cfg.CreateMap<IEnumerable<HttpPostedFileBase>, IEnumerable<FileDTO>>().ConvertUsing(x =>
                    {
                        List<FileDTO> rez = new List<FileDTO>();

                        foreach (var file in x)
                        {
                            if (file != null) if (file.ContentLength > 0)
                                {
                                    byte[] binaryData;

                                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                                    {
                                        binaryData = reader.ReadBytes(file.ContentLength);
                                    }

                                    rez.Add(new FileDTO()
                                    {
                                        Name = file.FileName,
                                        BinaryData = binaryData,
                                        FileType = "audio/*"
                                    });
                                }
                        }
                        return rez;
                    });
                });
            }
        }

        public static MapperConfiguration DTOtoContentWEBModelConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ContentDTO, ContentViewModel>();
                    cfg.CreateMap<GenreDTO, GenreModel>();
                    cfg.CreateMap<FileDTO, ShortFileModel>();
                    cfg.CreateMap<UserDTO, AboutUserModel>();
                });
            }
        }

        public static MapperConfiguration FileDTOtoFileModelConfig
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<FileDTO, FileModel>());
            }
        }

        public static MapperConfiguration ContentDTOtoContentUploadModelConfig
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ContentDTO, ContentUploadModel>();
                    cfg.CreateMap<GenreDTO, GenreModel>();
                    cfg.CreateMap<FileDTO, ShortFileModel>();
                    cfg.CreateMap<UserDTO, AboutUserModel>();
                }
                );
            }
        }

        public static MapperConfiguration GenreModelToGenreDTO
        {
            get
            {
                return new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<GenreModel, GenreDTO>();
                    cfg.CreateMap<GenreDTO, GenreModel>();
                });
            }
        }
    }
}
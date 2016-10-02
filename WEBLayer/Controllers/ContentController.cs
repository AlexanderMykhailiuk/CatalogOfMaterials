using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.DataTransferObjects;
using WEBLayer.Models;
using static WEBLayer.Mapping.MappingConfigs;

namespace WEBLayer.Controllers
{
    public class ContentController : Controller
    {
        readonly IContentService contentService;

        public ContentController(IContentService contentService)
        {
            this.contentService = contentService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Upload()
        {
            var mapper = GenreDTOtoGenreModelConfig.CreateMapper();
            ViewBag.Genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Upload(ContentUploadModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = ContentUploadModelToDTOConfig.CreateMapper();
                contentService.AddContent(User.Identity.Name,
                    mapper.Map<GenreDTO>(model),
                    mapper.Map<ContentDTO>(model),
                    mapper.Map<IEnumerable<FileDTO>>(model),
                    mapper.Map<FileDTO>(model)
                    );
                return RedirectToAction("Index", "Home");
            }
            else
            {

                var mapper = GenreDTOtoGenreModelConfig.CreateMapper();
                ViewBag.Genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ShowContentDetails(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                try
                {
                    int ContentID = id ?? -1;
                    var mapper = DTOtoContentWEBModelConfig.CreateMapper();

                    GenreDTO genre;
                    IEnumerable<FileDTO> files;
                    UserDTO user;

                    ContentViewModel content = mapper.Map<ContentViewModel>(
                        contentService.GetContentByIdWithAllData(ContentID, out genre, out files, out user)
                    );

                    ViewBag.Files = mapper.Map<IEnumerable<ShortFileModel>>(files);
                    ViewBag.Genre = mapper.Map<GenreModel>(genre);
                    ViewBag.User = mapper.Map<AboutUserModel>(user);

                    return View(content);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistContentException)
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpGet]
        public ActionResult GetFile(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                int FileID = id ?? -1;

                var mapper = FileDTOtoFileModelConfig.CreateMapper();

                var file = mapper.Map<FileModel>(contentService.GetFileById(FileID));

                if (file == null) return HttpNotFound();
                else
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        return File(file.BinaryData, file.FileType, file.Name);
                    }
                    else
                    {
                        if (string.Equals(file.FileType, "image/*")) return File(file.BinaryData, file.FileType, file.Name);
                        else return new HttpStatusCodeResult(401);
                    }
                }
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Search()
        {
            var mapper = GenreDTOtoGenreModelConfig.CreateMapper();
            ViewBag.Genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchResult(string searchingText, int? GenreID)
        {
            var mapper = ContentDTOtoShortContentModelConfig.CreateMapper();

            IEnumerable<ShortContentModel> searchContents = mapper.Map<IEnumerable<ShortContentModel>>(
                contentService.GetSearchingContents(searchingText, GenreID));

            return PartialView(searchContents);
        }

        [Authorize]
        public ActionResult AutocompleteSearch(string term)
        {
            var mapper = ContentDTOtoShortContentModelConfig.CreateMapper();

            IEnumerable<ShortContentModel> contents = mapper.Map<IEnumerable<ShortContentModel>>(contentService.GetAllContents());

            var possibleContents = from cont in contents
                                   where cont.Name.Contains(term)
                                   select new { value = cont.Name };

            return Json(possibleContents, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ManageUploads()
        {
            var mapper = ContentDTOtoShortContentModelConfig.CreateMapper();

            IEnumerable<ShortContentModel> userContents = mapper.Map<IEnumerable<ShortContentModel>>(
                contentService.GetContentsOfUser(User.Identity.Name));

            return View(userContents);
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteContent(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                int ContentID = id ?? -1;

                try
                {
                    contentService.Delete(User.Identity.Name, ContentID);
                    return Redirect(Request.UrlReferrer.ToString());
                }
                catch (BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistUserException)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistContentException)
                {
                    return HttpNotFound();
                }
                catch(UriFormatException)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateContent(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                try
                {
                    int ContentID = id ?? -1;

                    var mapper = ContentDTOtoContentUploadModelConfig.CreateMapper();
                    
                    GenreDTO genre;
                    IEnumerable<FileDTO> files;
                    UserDTO user;

                    ContentUploadModel content = mapper.Map<ContentUploadModel>(
                        contentService.GetContentByIdWithAllData(ContentID, out genre, out files, out user)
                    );

                    GenreModel choosenGenre = mapper.Map<GenreModel>(genre);
                    content.GenreID = choosenGenre.GenreID;
                    
                    ViewBag.Files = mapper.Map<IEnumerable<ShortFileModel>>(files);
                    ViewBag.Genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
                    ViewBag.User = mapper.Map<AboutUserModel>(user);

                    return View(content);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistContentException)
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateContent(int? id, ContentUploadModel model)
        {
            if (id == null) return HttpNotFound();
            else
            {
                try
                {
                    int ContentID = id ?? -1;

                    if (ModelState.IsValid)
                    {
                        var mapper = ContentUploadModelToDTOConfig.CreateMapper();
                        ContentDTO contentDTO = mapper.Map<ContentDTO>(model);

                        contentDTO.ContentID = ContentID;

                        contentService.Update(User.Identity.Name,
                            mapper.Map<GenreDTO>(model),
                            contentDTO,
                            mapper.Map<IEnumerable<FileDTO>>(model),
                            mapper.Map<FileDTO>(model)
                            );
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {   
                        return RedirectToAction("UpdateContent/" + ContentID,"Content");
                    }
                }
                catch(BusinessLogicLayer.Exceptions.NotExistUserException)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch(BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistContentException)
                {
                    return HttpNotFound();
                }
            }       
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Modertor")]
        public ActionResult ManageContents()
        {
            var mapper = ContentDTOtoShortContentModelConfig.CreateMapper();

            IEnumerable<ShortContentModel> Contents = mapper.Map<IEnumerable<ShortContentModel>>(
                contentService.GetAllContents());

            return View("ManageUploads", Contents);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Modertor")]
        public ActionResult ManageGenres()
        {
            var mapper = GenreDTOtoGenreModelConfig.CreateMapper();

            IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());

            return View(genres);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddGenre(string NameNewGenre)
        {
            var mapper = GenreModelToGenreDTO.CreateMapper();

            try
            {
                contentService.AddGenre(User.Identity.Name, mapper.Map<GenreDTO>(new GenreModel() { Name = NameNewGenre }));
                
                IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
                
                return View("ManageGenres",genres);
            }
            catch (BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
            {
                return new HttpStatusCodeResult(401);
            }
            catch (BusinessLogicLayer.Exceptions.NotExistUserException)
            {
                return new HttpStatusCodeResult(401);
            }
            catch (BusinessLogicLayer.Exceptions.AlreadyExistGenreException ex)
            {
                IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
                ViewBag.ErrorMessage = ex.Message;
                return View("ManageGenres", genres);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult RenameGenre(int?id, string NewGenreName)
        {
            if (id == null) return HttpNotFound();
            else
            {
                var mapper = GenreModelToGenreDTO.CreateMapper();

                int genreID = id ?? -1;

                try
                {
                    contentService.RenameGenre(User.Identity.Name, mapper.Map<GenreDTO>(new GenreModel() { GenreID = genreID, Name = NewGenreName }));

                    IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());

                    return PartialView("ManageGenresResult", genres);
                }
                catch (BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistUserException)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.AlreadyExistGenreException ex)
                {
                    IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
                    ViewBag.ErrorMessage = ex.Message;
                    return PartialView("ManageGenresResult", genres);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistGenreException)
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveGenre(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                var mapper = GenreModelToGenreDTO.CreateMapper();

                int genreID = id ?? -1;

                try
                {
                    contentService.DeleteGenre(User.Identity.Name, mapper.Map<GenreDTO>(new GenreModel() { GenreID = genreID }));

                    IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());

                    return PartialView("ManageGenresResult", genres);
                }
                catch (BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistUserException)
                {
                    return new HttpStatusCodeResult(401);
                }
                catch (BusinessLogicLayer.Exceptions.GenreContainContentsException ex)
                {
                    IEnumerable<GenreModel> genres = mapper.Map<IEnumerable<GenreModel>>(contentService.GetAllGenres());
                    ViewBag.ErrorMessage = ex.Message;
                    return PartialView("ManageGenresResult", genres);
                }
                catch (BusinessLogicLayer.Exceptions.NotExistGenreException)
                {
                    return HttpNotFound();
                }
            }
        }
    }
}
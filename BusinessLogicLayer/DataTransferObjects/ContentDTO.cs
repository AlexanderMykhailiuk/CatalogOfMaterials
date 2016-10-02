namespace BusinessLogicLayer.DataTransferObjects
{
    public class ContentDTO
    {
        public int ContentID { get; set; }
        public string Name { get; set; }
        public int YearOfCreation { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int? ImageID { get; set; }
    }
}

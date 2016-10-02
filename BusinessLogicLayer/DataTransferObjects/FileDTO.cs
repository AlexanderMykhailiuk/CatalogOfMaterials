namespace BusinessLogicLayer.DataTransferObjects
{
    public class FileDTO
    {
        public int FileID { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public byte[] BinaryData { get; set; }
    }
}

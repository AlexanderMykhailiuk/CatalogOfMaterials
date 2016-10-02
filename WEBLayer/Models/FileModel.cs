namespace WEBLayer.Models
{
    public class FileModel
    {
        public int FileID { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public byte[] BinaryData { get; set; }
    }
}
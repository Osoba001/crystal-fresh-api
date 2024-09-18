namespace Share.FileManagement
{
    public class ObjectStorageConfiguration
    {
        public required string SecretKey { get; set; }
        public required string AccessKey { get; set; }
        public required string StorageName { get; set; }
    }
}

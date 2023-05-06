using System;
using System.IO;
using System.Text.Json.Serialization;

namespace Discord
{
    public class PartialDiscordAttachment
    {
        public PartialDiscordAttachment(string path, string description = null)
            : this(System.IO.File.ReadAllBytes(path), path, description) { }

        public PartialDiscordAttachment(byte[] fileData, string fileName, string description = null)
            : this(new DiscordAttachmentFile(fileData), fileName, description) { }

        public PartialDiscordAttachment(DiscordAttachmentFile file, string fileName, string description = null)
        {
            if (file == null)
                throw new ArgumentException("May not be null.", nameof(file));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("May not be null or empty.", nameof(fileName));

            File = file;
            FileName = fileName;
            Description = description;
        }

        [JsonPropertyName("id")]
        internal ulong Id { get; set; }

        private string _fileName;
        [JsonPropertyName("filename")]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = Path.GetFileName(value); }
        }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonIgnore]
        public DiscordAttachmentFile File { get; set; }
    }
}

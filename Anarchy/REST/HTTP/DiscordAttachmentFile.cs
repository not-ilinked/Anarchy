using System;
using Microsoft.Maui.Graphics;

namespace Discord
{
    public class DiscordAttachmentFile
    {
        public DiscordAttachmentFile(byte[] bytes, ImageFormat imageFormat)
            : this(bytes, imageFormat.ToMediaType()) { }

        public DiscordAttachmentFile(byte[] bytes, string mediaType = null)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("May not be null and Length must be > 0.", nameof(bytes));

            Bytes = bytes;
            MediaType = mediaType;
        }

        public bool IsImage()
        {
            return DiscordImageMediaType.IsSupportedImageFormat(MediaType);
        }

        public byte[] Bytes { get; }
        public string MediaType { get; }
    }
}

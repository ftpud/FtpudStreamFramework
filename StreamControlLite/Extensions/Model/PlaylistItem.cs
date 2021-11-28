using FtpudStreamFramewok.Source;

namespace StreamControlLite.Extensions.Model
{
    public class PlaylistItem
    {
        public PlaylistItem(FileSourceEntity fileSource)
        {
            _sourceEntity = fileSource;
        }

        private FileSourceEntity _sourceEntity;

        public string FileName => _sourceEntity.GetFileName();

        public SourceEntity GetSourceEntity()
        {
            return _sourceEntity;
        }
    }
}
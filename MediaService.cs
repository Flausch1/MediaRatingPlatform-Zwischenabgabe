using System;
using System.Collections.Generic;
using System.Text;


using HttpServerDemo.Data;

namespace HttpServerDemo
{
    internal class MediaService
    {
        private IMediaRepository mediaRepository;

        public MediaService(IMediaRepository repository)
        {
            mediaRepository = repository;
        }

        public bool Create(Media media, int creatorId)
        {
            media.CreatorId = creatorId;

            return mediaRepository.Create(media);
        }

        public List<Media> GetAll()
        {
            return mediaRepository.GetAll();
        }

        public Media? GetById(int id)
        {
            return mediaRepository.GetById(id);
        }

        public bool Update(Media media, int creatorId)
        {
            media.CreatorId = creatorId;

            return mediaRepository.Update(media);
        }

        public bool DeleteById(int id, int creatorId)
        {
            return mediaRepository.DeleteById(id, creatorId);
        }
    }
}

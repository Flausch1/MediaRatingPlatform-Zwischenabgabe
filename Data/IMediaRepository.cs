using System;
using System.Collections.Generic;
using System.Text;


using System.Collections.Generic;

namespace HttpServerDemo.Data
{
    internal interface IMediaRepository
    {
        bool Create(Media media);
        bool Update(Media media);
        bool DeleteById(int id, int creatorId);
        Media? GetById(int id);
        List<Media> GetAll();
    }
}

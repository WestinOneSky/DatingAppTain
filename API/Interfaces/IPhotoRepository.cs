using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPhotoRepository
    {
        public Task<Photo> GetPhotoById(int id);
        public Task<IEnumerable<PhotoForApprovalDto>> GetPhotos();
        void DeletePhoto(Photo photo);
    }
}
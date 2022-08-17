using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        public readonly DataContext2 _context2;
        public PhotoRepository(DataContext2 context2)
        {
            _context2 = context2;

        }
        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context2.Photos.IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Id == id);
        }
        public async Task<IEnumerable<PhotoForApprovalDto>> GetPhotos()
        {
            return await _context2.Photos
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .Select(u => new PhotoForApprovalDto
                {
                    Id = u.Id,
                    Url = u.Url,
                    Username = u.AppUser.UserName,
                    IsApproved = u.IsApproved
                })
                .ToListAsync();
        }
        public void DeletePhoto(Photo photo)
        {
            _context2.Photos.Remove(photo);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public DataContext2 _context2;
        public IMapper _mapper;
        public UnitOfWork(DataContext2 context2, IMapper mapper)
        {
            _mapper = mapper;
            _context2 = context2;
        }

        public IUserRepository UserRepository => new UserRepository(_context2, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_context2, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_context2);

        public async Task<bool> Complete()
        {
            return await _context2.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context2.ChangeTracker.HasChanges();
        }
    }
}
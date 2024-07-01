using AutoMapper;
using BooksApiApp.Repositories;
using BooksApiApp.Data;
using BooksApiApp.Security;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        //private readonly IMapper _mapper;

        public UserRepository(BooksWebApiContext context)
            : base(context)
        {
            //_mapper = mapper;
        }


        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.Where(x => x.Username == username && !x.IsDeleted).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await _context.Users
                        .Where(x => (x.Username == username || x.Email == username) && !x.IsDeleted)
                        .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            if (!EncryptionUtil.IsValidPassword(password, user.Password))
            {
                return null;
            }
            return user;
        }

        public async Task<User?> UpdateUserAsync(int userId, User user)
        {
            var existingUser = await _context.Users
                                .Where(x => x.Id == userId && !x.IsDeleted)
                                .FirstOrDefaultAsync();

            if (existingUser is null) return null;
            if (existingUser.Id != userId) return null;

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            _context.Entry(existingUser).State = EntityState.Modified;

            return existingUser;

        }

        public async Task<User?> GetByPhoneNumber(string phoneNumber)
        {
            return await _context.Users
                         .Where(x => x.PhoneNumber == phoneNumber && !x.IsDeleted)
                         .FirstOrDefaultAsync();
        }


        public override async Task<bool> DeleteAsync(int id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser is not null)
            {
                existingUser.IsDeleted = true;
                _context.Users.Update(existingUser);
                return true;
            }
            return false;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            var entities = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            return entities;
        }

        public override async Task<User?> GetAsync(int id)
        {
            var entity = await _context.Users.Where(x => x.Id == id && !x.IsDeleted)
                               .FirstOrDefaultAsync();
            return entity;
        }

        public override async Task<int> GetCountAsync()
        {
            var count = await _context.Users.Where(u => !u.IsDeleted).CountAsync();
            return count;
        }


    }
}

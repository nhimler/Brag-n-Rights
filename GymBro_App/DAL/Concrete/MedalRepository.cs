using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GymBro_App.DAL.Concrete
{
    public class MedalRepository : IMedalRepository
    {
        private readonly GymBroDbContext _context;

        public MedalRepository(GymBroDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medal>> GetAllMedalsAsync()
        {
            return await _context.Medals.ToListAsync();
        }

        public Medal FindById(int id)
        {
            return _context.Medals.Find(id);
        }

        public bool Exists(int id)
        {
            return _context.Medals.Any(m => m.MedalId == id);
        }

        public IQueryable<Medal> GetAll()
        {
            return _context.Medals.AsQueryable();
        }

        public IQueryable<Medal> GetAll(Expression<Func<Medal, bool>> predicate)
        {
            return _context.Medals.Where(predicate).AsQueryable();
        }

        public IQueryable<Medal> GetAll(params Expression<Func<Medal, object>>[] includeProperties)
        {
            IQueryable<Medal> query = _context.Medals;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Medal AddOrUpdate(Medal entity)
        {
            if (_context.Medals.Any(m => m.MedalId == entity.MedalId))
            {
                _context.Medals.Update(entity);
            }
            else
            {
                _context.Medals.Add(entity);
            }
            _context.SaveChanges();
            return entity;
        }

        public void Delete(Medal entity)
        {
            _context.Medals.Remove(entity);
            _context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = _context.Medals.Find(id);
            if (entity != null)
            {
                _context.Medals.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
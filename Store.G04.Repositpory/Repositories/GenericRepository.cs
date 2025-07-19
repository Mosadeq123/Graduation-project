using Microsoft.EntityFrameworkCore;
using Store.G04.Core;
using Store.G04.Core.Entities;
using Store.G04.Core.Repositories.Contract;
using Store.G04.Core.Specifications;
using Store.G04.Repositpory.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.G04.Repositpory.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        // الحصول على جميع الكيانات بشكل غير متزامن
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(MachineEntity))
            {
                return (IEnumerable<TEntity>)await _context.Machine.OrderBy(P => P.NameMachine).Include(p => p.RawMaterials).ToListAsync();
            }
            return await _context.Set<TEntity>().ToListAsync();
        }

        // الحصول على كيان معين باستخدام المعرف (id)
        public async Task<TEntity> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(MachineEntity))
            {
                return await _context.Machine.Include(p => p.RawMaterials).FirstOrDefaultAsync(p => p.Id == id as int?) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id);
        }

        // إضافة كيان جديد
        public async Task AddAsync(TEntity entity)
        {
           await _context.AddAsync(entity);
        }

        // تحديث كيان
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await SaveChangesAsync();
        }

        // حذف كيان
        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await SaveChangesAsync();
        }

        // حفظ التغييرات
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
           return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec);
        }

        public Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }
    }
}
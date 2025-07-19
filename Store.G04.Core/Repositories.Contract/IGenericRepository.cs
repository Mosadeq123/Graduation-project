using Store.G04.Core.Entities;
using Store.G04.Core.Specifications;

namespace Store.G04.Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        // الحصول على جميع الكيانات بشكل غير متزامن
        Task<IEnumerable<TEntity>> GetAllAsync();

        // الحصول على كيان معين باستخدام المعرف (id)
        Task<TEntity> GetAsync(TKey id);

        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        // إضافة كيان جديد

        Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec);
        Task AddAsync(TEntity entity);

        // تحديث كيان
        Task UpdateAsync(TEntity entity);

        // حذف كيان
        Task DeleteAsync(TEntity entity);

        // حفظ التغييرات بشكل غير متزامن
        Task SaveChangesAsync();

    }
}

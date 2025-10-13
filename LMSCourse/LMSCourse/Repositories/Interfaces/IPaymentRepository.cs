using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByAppTransId(string appTransId);
        IQueryable<Payment> GetQueryable();
        void UpdateAsyncNoSave(Payment payment);
        Task SaveChangeAsync();
    }
}

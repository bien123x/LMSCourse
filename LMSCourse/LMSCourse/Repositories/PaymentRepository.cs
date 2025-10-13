using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Payment?> GetByAppTransId(string appTransId)
        {
            return await _context.Payments
                .Include(p => p.PaymentDetails)
                    .ThenInclude(pd => pd.Course)
                .FirstOrDefaultAsync(p => p.AppTransId == appTransId);
        }

        public IQueryable<Payment> GetQueryable()
        {
            return _context.Payments.Include(p => p.PaymentDetails).AsQueryable();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateAsyncNoSave(Payment payment)
        {
             _context.Update(payment);
        }
    }
}

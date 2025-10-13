using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSCourse.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        
        public long Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        //'Pending', 'Paid', 'Failed'
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
        public string AppTransId { get; set; } = string.Empty;
    }

}

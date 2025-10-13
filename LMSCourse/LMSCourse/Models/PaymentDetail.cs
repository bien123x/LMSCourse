namespace LMSCourse.Models
{
    public class PaymentDetail
    {
        public int PaymentDetailId { get; set; }

        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public long? Price { get; set; } // giá tại thời điểm thanh toán
    }
}

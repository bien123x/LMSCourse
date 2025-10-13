namespace LMSCourse.DTOs.ZaloPay
{
    public class ZaloPayCallbackDto
    {
        public string data { get; set; }
        public string mac { get; set; }
        public int type { get; set; }
    }
}

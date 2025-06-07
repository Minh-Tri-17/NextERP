namespace NextERP.Util
{
    public class Enums
    {
        public enum Color
        {
            Primary,
            Danger,
            Info,
            Success,
            Warning,
        }

        public enum GroupRole
        {
            /// <summary>
            /// Nhóm quyền dành cho khách hàng
            /// </summary>
            Customer,
            /// <summary>
            /// Nhóm quyền dành cho nhân viên
            /// </summary>
            Employee,
            /// <summary>
            /// Nhóm quyền dành cho quản lý
            /// </summary>
            Admin,
        }

        public enum AppointmentStatus
        {
            /// <summary>
            /// Lịch hẹn đã được đặt
            /// </summary>
            Booked,
            /// <summary>
            /// Lịch hẹn đã được hoàn thành
            /// </summary>
            Completed,
            /// <summary>
            /// Lịch hẹn đã bị hủy
            /// </summary>
            Cancelled,
            /// <summary>
            /// Lịch hẹn đã bị bỏ lỡ
            /// </summary>
            Missed,
            /// <summary>
            /// Lịch hẹn đã được lên lại
            /// </summary>
            Rescheduled,
            /// <summary>
            /// Lich hẹn đang được xử lý
            /// </summary>
            InProgress
        }
    }
}

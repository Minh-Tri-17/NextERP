namespace NextERP.Util
{
    public class Enums
    {
        public enum Color
        {
            /// <summary>
            /// Màu sắc mặc định
            /// </summary>
            ColorPrimary,
            /// <summary>
            /// Màu sắc cảnh báo lỗi
            /// </summary>
            ColorDanger,
            /// <summary>
            /// Màu sắc thông tin
            /// </summary>
            ColorInfo,
            /// <summary>
            /// Màu sắc thành công
            /// </summary>
            ColorSuccess,
            /// <summary>
            /// Màu sắc cảnh báo
            /// </summary>
            ColorWarning,
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
            AppointmentBooked,
            /// <summary>
            /// Lịch hẹn đã được hoàn thành
            /// </summary>
            AppointmentCompleted,
            /// <summary>
            /// Lịch hẹn đã bị hủy
            /// </summary>
            AppointmentCancelled,
            /// <summary>
            /// Lịch hẹn đã bị bỏ lỡ
            /// </summary>
            AppointmentMissed,
            /// <summary>
            /// Lịch hẹn đã được lên lại
            /// </summary>
            AppointmentRescheduled,
            /// <summary>
            /// Lich hẹn đang được xử lý
            /// </summary>
            AppointmentInProgress
        }

        public enum Gender
        {
            /// <summary>
            /// Nam
            /// </summary>
            Male,
            /// <summary>
            /// Nữ
            /// </summary>
            Female,
            /// <summary>
            /// Khác
            /// </summary>
            Other
        }

        public enum EducationLevel
        {
            /// <summary>
            /// Không xác định
            /// </summary>
            Unknown,
            /// <summary>
            /// Tiểu học
            /// </summary>
            PrimarySchool,
            /// <summary>
            /// Trung học cơ sở
            /// </summary>
            SecondarySchool,
            /// <summary>
            /// Trung hoc phổ thông
            /// </summary>
            HighSchool,
            /// <summary>
            /// Trung cấp
            /// </summary>
            Intermediate,
            /// <summary>
            /// Cao đẳng
            /// </summary>
            College,
            /// <summary>
            /// Đại học
            /// </summary>
            University,
            /// <summary>
            /// Thạc sĩ
            /// </summary>
            Master,
            /// <summary>
            /// Tiền sĩ
            /// </summary>
            Doctor,
            /// <summary>
            /// Giáo sư
            /// </summary>
            Professor
        }

        public enum OperatingStatus
        {
            /// <summary>
            /// Đang hoạt động
            /// </summary>
            Active,
            /// <summary>
            /// Ngưng hoạt động
            /// </summary>
            Inactive
        }

        public enum Permissions
        {
            /// <summary>
            /// Quyền xem thông tin
            /// </summary>
            View,
            /// <summary>
            /// Quyền thêm mới
            /// </summary>
            Create,
            /// <summary>
            /// Quyền sửa đổi
            /// </summary>
            Edit,
            /// <summary>
            /// Quyền xóa
            /// </summary>
            Delete,
            /// <summary>
            /// Quyền xóa vĩnh viễn
            /// </summary>
            DeletePermanently,
            /// <summary>
            /// Quyền import dữ liệu
            /// </summary>
            Import,
            /// <summary>
            /// Quyền xuất dữ liệu
            /// </summary>
            Export
        }
    }
}

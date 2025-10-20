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

        public enum SessionStatus
        {
            /// <summary>
            /// Buổi đào tạo đang diễn ra
            /// </summary>
            SessionOngoing,

            /// <summary>
            /// Buổi đào tạo đã kết thúc
            /// </summary>
            SessionCompleted,

            /// <summary>
            /// Buổi đào tạo đã bị hủy
            /// </summary>
            SessionCancelled
        }

        public enum SpaServiceLevel
        {
            /// <summary>
            /// Dịch vụ cơ bản
            /// </summary>
            Basic,

            /// <summary>
            /// Dịch vụ nâng cao
            /// </summary>
            Advanced,

            /// <summary>
            /// Dịch vụ cao cấp
            /// </summary>
            Premium
        }

        public enum ApprovalStatus
        {
            /// <summary>
            /// Đã yêu cầu
            /// </summary>
            Submitted,

            /// <summary>
            /// Đang được xem xét
            /// </summary>
            InReview,

            /// <summary>
            /// Đã duyệt
            /// </summary>
            Approved,

            /// <summary>
            /// Đã hủy
            /// </summary>
            Cancelled,

            /// <summary>
            /// Đã từ chối
            /// </summary>
            Rejected
        }

        public enum LeaveDayType
        {
            /// <summary>
            /// Nghỉ phép năm (được hưởng lương theo chế độ)
            /// </summary>
            AnnualLeave,

            /// <summary>
            /// Nghỉ bệnh (phải có giấy xác nhận nếu dài ngày)
            /// </summary>
            SickLeave,

            /// <summary>
            /// Nghỉ không lương (tự nguyện hoặc lý do cá nhân)
            /// </summary>
            UnpaidLeave,

            /// <summary>
            /// Nghỉ thai sản dành cho nhân viên nữ
            /// </summary>
            MaternityLeave,

            /// <summary>
            /// Nghỉ do lý do cá nhân (đám cưới, tang lễ, con ốm,...) có thể được hưởng lương
            /// </summary>
            PersonalLeave,

            /// <summary>
            /// Nghỉ phép bù (nghỉ bù do làm thêm, lễ,...)
            /// </summary>
            CompensatoryLeave
        }

        public enum OrderStatus
        {
            /// <summary>
            /// Đang xử lý
            /// </summary>
            Processing,

            /// <summary>
            ///  Đã giao
            /// </summary>            
            Delivered,

            /// <summary>
            ///  Đã hủy
            /// </summary>
            Cancelled
        }

        public enum PaymentStatus
        {
            /// <summary>
            /// Chưa thanh toán
            /// </summary>
            Unpaid,

            /// <summary>
            /// Đã thanh toán
            /// </summary>
            Paid
        }

        public enum PaymentMethod
        {
            /// <summary>
            /// Tiền mặt
            /// </summary>
            Cash,

            /// <summary>
            /// Chuyển khoản
            /// </summary>
            CreditCard,
        }

        public enum FilterOperator
        {
            /// <summary>
            /// So khớp tương đối, thường dùng cho string (SQL: LIKE '%value%')
            /// </summary>
            Like,

            /// <summary>
            /// Bằng nhau (=)
            /// </summary>
            Equal,

            /// <summary>
            /// Khác nhau (!=)
            /// </summary>
            NotEqual,

            /// <summary>
            /// Lớn hơn (>)
            /// </summary>
            GreaterThan,

            /// <summary>
            /// Lớn hơn hoặc bằng (>=)
            /// </summary>
            GreaterOrEqual,

            /// <summary>
            /// Nhỏ hơn (<)
            /// </summary>
            LessThan,

            /// <summary>
            /// Nhỏ hơn hoặc bằng (<=)
            /// </summary>
            LessOrEqual,

            /// <summary>
            /// Trong khoảng từ ... đến ... (áp dụng cho số hoặc ngày)
            /// </summary>
            Between,

            /// <summary>
            /// Chuỗi chứa giá trị con (string.Contains) → SQL: LIKE '%value%'
            /// </summary>
            Contains
        }

        public enum FilterType
        {
            String,
            Number,
            Date,
            Boolean,
            Guid,
        }
    }
}

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

        public enum Role
        {
            Customer,
            Manager,
            Employee,
            Admin,
            Developer
        }

        public enum AppointmentStatus
        {
            Booked,
            Completed,
            Cancelled,
            Missed,
            Rescheduled,
            InProgress
        }
    }
}

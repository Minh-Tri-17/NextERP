using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Employee
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? EmployeeCode { get; set; }

    [StringLength(50)]
    public string FullName { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(5)]
    public string? Gender { get; set; }

    [Column("DOB", TypeName = "datetime")]
    public DateTime? Dob { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [Column("NationalID")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NationalId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(150)]
    public string? Photo { get; set; }

    [StringLength(50)]
    public string? EducationLevel { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BankAccountNumber { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HealthInsuranceNumber { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SocialInsuranceNumber { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TaxCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? HireDate { get; set; }

    [StringLength(50)]
    public string? OperatingStatus { get; set; }

    [Column("PositionID")]
    public Guid? PositionId { get; set; }

    [Column("DepartmentID")]
    public Guid? DepartmentId { get; set; }

    [Column("BranchID")]
    public Guid? BranchId { get; set; }

    public bool? IsDelete { get; set; }

    [StringLength(1500)]
    public string? Note { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateCreate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateUpdate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UserCreate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UserUpdate { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [InverseProperty("Employee")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("BranchId")]
    [InverseProperty("Employees")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Employees")]
    public virtual Department? Department { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<HistoryMail> HistoryMails { get; set; } = new List<HistoryMail>();

    [InverseProperty("Employee")]
    public virtual ICollection<HistoryNotification> HistoryNotifications { get; set; } = new List<HistoryNotification>();

    [InverseProperty("Employee")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("Employee")]
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    [ForeignKey("PositionId")]
    [InverseProperty("Employees")]
    public virtual Position? Position { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    [InverseProperty("Employee")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [InverseProperty("Employee")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

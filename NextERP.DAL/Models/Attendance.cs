using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Attendance
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? AttendanceCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? WorkDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OutTime { get; set; }

    public int? WorkingHours { get; set; }

    public int? OvertimeHours { get; set; }

    [Column("EmployeeID")]
    public Guid? EmployeeId { get; set; }

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

    [ForeignKey("EmployeeId")]
    [InverseProperty("Attendances")]
    public virtual Employee? Employee { get; set; }
}

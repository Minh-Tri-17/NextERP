using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.API.Models;

public partial class LeaveRequest
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? LeaveRequestCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LeaveStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LeaveEndDate { get; set; }

    public int? TotalLeaveDays { get; set; }

    [StringLength(50)]
    public string? LeaveDayType { get; set; }

    [StringLength(50)]
    public string? RequestStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprovalDate { get; set; }

    [Column("ApprovedByIDs")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ApprovedByIds { get; set; }

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
    [InverseProperty("LeaveRequests")]
    public virtual Employee? Employee { get; set; }
}

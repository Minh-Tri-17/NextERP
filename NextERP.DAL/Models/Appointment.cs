using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Appointment
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? AppointmentCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AppointmentDate { get; set; }

    [StringLength(50)]
    public string? AppointmentStatus { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalCost { get; set; }

    [Column("CustomerID")]
    public Guid? CustomerId { get; set; }

    [Column("EmployeeID")]
    public Guid? EmployeeId { get; set; }

    [Column("ServiceID")]
    public Guid? ServiceId { get; set; }

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

    [ForeignKey("BranchId")]
    [InverseProperty("Appointments")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Appointments")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Appointments")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("Appointments")]
    public virtual SpaService? Service { get; set; }
}

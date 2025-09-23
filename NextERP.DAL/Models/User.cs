using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class User
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? UserCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? PasswordHash { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }

    [StringLength(50)]
    public string? OperatingStatus { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? GroupRole { get; set; }

    [Column("EmployeeID")]
    public Guid? EmployeeId { get; set; }

    [Column("CustomerID")]
    public Guid? CustomerId { get; set; }

    [Column("RoleIDs")]
    [StringLength(500)]
    [Unicode(false)]
    public string? RoleIds { get; set; }

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

    [ForeignKey("CustomerId")]
    [InverseProperty("Users")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Users")]
    public virtual Employee? Employee { get; set; }
}

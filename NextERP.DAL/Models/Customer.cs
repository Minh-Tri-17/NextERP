using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Customer
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? CustomerCode { get; set; }

    [StringLength(50)]
    public string? FullName { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [Column("DOB", TypeName = "datetime")]
    public DateTime? Dob { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinDate { get; set; }

    public int? LoyaltyPoints { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalSpent { get; set; }

    [StringLength(50)]
    public string? OperatingStatus { get; set; }

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

    [InverseProperty("Customer")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [InverseProperty("Customer")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Customer")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("Customer")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Service
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? ServiceCode { get; set; }

    [StringLength(150)]
    public string? ServiceName { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Price { get; set; }

    public int? Duration { get; set; }

    [StringLength(50)]
    public string? OperatingStatus { get; set; }

    public bool? IsPromotional { get; set; }

    [MaxLength(500)]
    public byte[]? ServiceImage { get; set; }

    [StringLength(50)]
    public string? ServiceLevel { get; set; }

    [Column("SuppliesRequiredIDs")]
    [StringLength(500)]
    [Unicode(false)]
    public string? SuppliesRequiredIds { get; set; }

    [Column("ServiceCategoryID")]
    public Guid? ServiceCategoryId { get; set; }

    [Column("PromotionID")]
    public Guid? PromotionId { get; set; }

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

    [InverseProperty("Service")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [InverseProperty("Service")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Service")]
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    [ForeignKey("PromotionId")]
    [InverseProperty("Services")]
    public virtual Promotion? Promotion { get; set; }

    [ForeignKey("ServiceCategoryId")]
    [InverseProperty("Services")]
    public virtual ServiceCategory? ServiceCategory { get; set; }
}

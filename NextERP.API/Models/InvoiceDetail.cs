using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.API.Models;

public partial class InvoiceDetail
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? InvoiceDetailCode { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalPrice { get; set; }

    [Column("InvoiceID")]
    public Guid? InvoiceId { get; set; }

    [Column("ProductID")]
    public Guid? ProductId { get; set; }

    [Column("ServiceID")]
    public Guid? ServiceId { get; set; }

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

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceDetails")]
    public virtual Invoice? Invoice { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("InvoiceDetails")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("InvoiceDetails")]
    public virtual SpaService? Service { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.API.Models;

public partial class SupplierOrderDetail
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? SupplierOrderDetailCode { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalPrice { get; set; }

    [Column("SupplierOrderID")]
    public Guid? SupplierOrderId { get; set; }

    [Column("ProductID")]
    public Guid? ProductId { get; set; }

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

    [ForeignKey("ProductId")]
    [InverseProperty("SupplierOrderDetails")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SupplierOrderId")]
    [InverseProperty("SupplierOrderDetails")]
    public virtual SupplierOrder? SupplierOrder { get; set; }
}

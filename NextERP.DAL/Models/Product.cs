using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Product
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? ProductCode { get; set; }

    [StringLength(150)]
    public string? ProductName { get; set; }

    public int? QuantityInStock { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpirationDate { get; set; }

    [StringLength(50)]
    public string? Manufacturer { get; set; }

    [StringLength(50)]
    public string? CountryOfOrigin { get; set; }

    public int? ReorderLevel { get; set; }

    [Column("SupplierID")]
    public Guid? SupplierId { get; set; }

    [Column("CategoryID")]
    public Guid? CategoryId { get; set; }

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

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual ProductCategory? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier? Supplier { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; } = new List<SupplierOrderDetail>();
}

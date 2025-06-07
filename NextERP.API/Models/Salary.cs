using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.API.Models;

public partial class Salary
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? SalaryCode { get; set; }

    public int? SalaryMonth { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? GrossSalary { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Bonus { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TaxAmount { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? InsuranceContribution { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AdvanceSalary { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Deductions { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? NetSalary { get; set; }

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
    [InverseProperty("Salaries")]
    public virtual Employee? Employee { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class HistoryMail
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? HistoryMailCode { get; set; }

    [StringLength(150)]
    public string? HistoryMailName { get; set; }

    [StringLength(500)]
    public string? HistoryMailContent { get; set; }

    [Column("EmployeeID")]
    public Guid? EmployeeId { get; set; }

    [Column("TemplateMailID")]
    public Guid? TemplateMailId { get; set; }

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
    [InverseProperty("HistoryMails")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("TemplateMailId")]
    [InverseProperty("HistoryMails")]
    public virtual TemplateMail? TemplateMail { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class Mail
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? MailCode { get; set; }

    [StringLength(150)]
    public string? MailName { get; set; }

    [StringLength(500)]
    public string? MailContent { get; set; }

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
    [InverseProperty("Mail")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("TemplateMailId")]
    [InverseProperty("Mail")]
    public virtual TemplateMail? TemplateMail { get; set; }
}

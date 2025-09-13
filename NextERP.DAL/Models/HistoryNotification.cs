using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class HistoryNotification
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? HistoryNotificationCode { get; set; }

    [StringLength(150)]
    public string? HistoryNotificationName { get; set; }

    [StringLength(500)]
    public string? HistoryNotificationContent { get; set; }

    [Column("EmployeeID")]
    public Guid? EmployeeId { get; set; }

    [Column("TemplateNotificationID")]
    public Guid? TemplateNotificationId { get; set; }

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
    [InverseProperty("HistoryNotifications")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("TemplateNotificationId")]
    [InverseProperty("HistoryNotifications")]
    public virtual TemplateNotification? TemplateNotification { get; set; }
}

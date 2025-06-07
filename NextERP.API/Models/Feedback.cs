using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.API.Models;

public partial class Feedback
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? FeedbackCode { get; set; }

    public int? Rating { get; set; }

    [StringLength(1500)]
    public string? Comment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateFeedback { get; set; }

    [Column("CustomerID")]
    public Guid? CustomerId { get; set; }

    [Column("ServiceID")]
    public Guid? ServiceId { get; set; }

    public bool? IsDelete { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Feedbacks")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("Feedbacks")]
    public virtual SpaService? Service { get; set; }
}

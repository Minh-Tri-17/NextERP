using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NextERP.API.Models;

public partial class TrainingSession
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? TrainingSessionCode { get; set; }

    [StringLength(150)]
    public string? TrainingSessionName { get; set; }

    [StringLength(50)]
    public string? Trainer { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TrainingDate { get; set; }

    public int? Duration { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Participants { get; set; }

    [StringLength(50)]
    public string? TrainingMaterial { get; set; }

    [StringLength(50)]
    public string? SessionStatus { get; set; }

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
}

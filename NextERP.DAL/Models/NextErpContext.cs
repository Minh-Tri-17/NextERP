using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NextERP.DAL.Models;

public partial class NextErpContext : DbContext
{
    public NextErpContext()
    {
    }

    public NextErpContext(DbContextOptions<NextErpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Function> Functions { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Mail> Mails { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SpaService> SpaServices { get; set; }

    public virtual DbSet<SpaServiceCategory> SpaServiceCategories { get; set; }

    public virtual DbSet<SpaServiceImage> SpaServiceImages { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }

    public virtual DbSet<SupplierOrderDetail> SupplierOrderDetails { get; set; }

    public virtual DbSet<TemplateMail> TemplateMails { get; set; }

    public virtual DbSet<TemplateNotification> TemplateNotifications { get; set; }

    public virtual DbSet<TrainingSession> TrainingSessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ASUS;Initial Catalog=NextERP;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.Appointments).HasConstraintName("FK_Appointments_Branches");

            entity.HasOne(d => d.Customer).WithMany(p => p.Appointments).HasConstraintName("FK_Appointments_Customers");

            entity.HasOne(d => d.Employee).WithMany(p => p.Appointments).HasConstraintName("FK_Appointments_Employees");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments).HasConstraintName("FK_Appointments_Services");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances).HasConstraintName("FK_Attendances_Employees");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Branches");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Departments");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Positions");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks).HasConstraintName("FK_Feedbacks_Customers");

            entity.HasOne(d => d.Service).WithMany(p => p.Feedbacks).HasConstraintName("FK_Feedbacks_Services");
        });

        modelBuilder.Entity<Function>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ParentFunction).WithMany(p => p.InverseParentFunction).HasConstraintName("FK_Functions_Functions");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.Invoices).HasConstraintName("FK_Invoices_Branches");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices).HasConstraintName("FK_Invoices_Customers");

            entity.HasOne(d => d.Employee).WithMany(p => p.Invoices).HasConstraintName("FK_Invoices_Employees");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails).HasConstraintName("FK_InvoiceDetails_Invoices");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceDetails).HasConstraintName("FK_InvoiceDetails_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.InvoiceDetails).HasConstraintName("FK_InvoiceDetails_Services");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests).HasConstraintName("FK_LeaveRequests_Employees");
        });

        modelBuilder.Entity<Mail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.Mail).HasConstraintName("FK_Mails_Employees");

            entity.HasOne(d => d.TemplateMail).WithMany(p => p.Mail).HasConstraintName("FK_Mails_TemplateMails");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.Notifications).HasConstraintName("FK_Notifications_Employees");

            entity.HasOne(d => d.TemplateNotification).WithMany(p => p.Notifications).HasConstraintName("FK_Notifications_TemplateNotifications");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_Products_ProductCategories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("FK_ProductImage_Products");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries).HasConstraintName("FK_Salaries_Employees");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.Schedules).HasConstraintName("FK_Schedules_Employees");
        });

        modelBuilder.Entity<SpaService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Services");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Promotion).WithMany(p => p.SpaServices).HasConstraintName("FK_Services_Promotions");

            entity.HasOne(d => d.SpaServiceCategory).WithMany(p => p.SpaServices).HasConstraintName("FK_Services_ServiceCategories");
        });

        modelBuilder.Entity<SpaServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ServiceCategories");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SpaServiceImage>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SpaService).WithMany(p => p.SpaServiceImages).HasConstraintName("FK_SpaServiceImage_SpaService");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SupplierOrder>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierOrders).HasConstraintName("FK_SupplierOrders_Suppliers");
        });

        modelBuilder.Entity<SupplierOrderDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.SupplierOrderDetails).HasConstraintName("FK_SupplierOrderDetails_Products");

            entity.HasOne(d => d.SupplierOrder).WithMany(p => p.SupplierOrderDetails).HasConstraintName("FK_SupplierOrderDetails_SupplierOrders");
        });

        modelBuilder.Entity<TemplateMail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TemplateMail");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TemplateNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TemplateNotification");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TrainingSession>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.Users).HasConstraintName("FK_Users_Customers");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users).HasConstraintName("FK_Users_Employees");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

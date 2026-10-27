using Microsoft.EntityFrameworkCore;
using Rsvp.Api.Models;

namespace Rsvp.Api.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<EmployeeService> EmployeeServices => Set<EmployeeService>();
    public DbSet<WorkingHour> WorkingHours => Set<WorkingHour>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(customer => customer.Email).IsUnique();
            entity.Property(customer => customer.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(customer => customer.LastName).HasMaxLength(100).IsRequired();
            entity.Property(customer => customer.Email).HasMaxLength(254).IsRequired();
            entity.Property(customer => customer.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(customer => customer.PhoneNumber).HasMaxLength(30);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(employee => employee.Email).IsUnique();
            entity.Property(employee => employee.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(employee => employee.LastName).HasMaxLength(100).IsRequired();
            entity.Property(employee => employee.Email).HasMaxLength(254).IsRequired();
            entity.Property(employee => employee.PasswordHash).HasMaxLength(512).IsRequired();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(service => service.Name).HasMaxLength(150).IsRequired();
            entity.Property(service => service.Description).HasMaxLength(1000);
            entity.Property(service => service.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<EmployeeService>(entity =>
        {
            entity.HasKey(employeeService =>
                new { employeeService.EmployeeId, employeeService.ServiceId });

            entity.HasOne(employeeService => employeeService.Employee)
                .WithMany(employee => employee.EmployeeServices)
                .HasForeignKey(employeeService => employeeService.EmployeeId);

            entity.HasOne(employeeService => employeeService.Service)
                .WithMany(service => service.EmployeeServices)
                .HasForeignKey(employeeService => employeeService.ServiceId);
        });

        modelBuilder.Entity<WorkingHour>(entity =>
        {
            entity.HasOne(workingHour => workingHour.Employee)
                .WithMany(employee => employee.WorkingHours)
                .HasForeignKey(workingHour => workingHour.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(reservation => reservation.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasOne(reservation => reservation.Customer)
                .WithMany(customer => customer.Reservations)
                .HasForeignKey(reservation => reservation.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(reservation => reservation.Employee)
                .WithMany(employee => employee.Reservations)
                .HasForeignKey(reservation => reservation.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(reservation => reservation.Service)
                .WithMany(service => service.Reservations)
                .HasForeignKey(reservation => reservation.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

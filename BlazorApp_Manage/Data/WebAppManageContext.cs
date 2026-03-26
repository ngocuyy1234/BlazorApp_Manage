using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp_Manage.Data;

public partial class WebAppManageContext : DbContext
{
    public WebAppManageContext()
    {
    }

    public WebAppManageContext(DbContextOptions<WebAppManageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ActionLog> ActionLogs { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceType> DeviceTypes { get; set; }

    public virtual DbSet<DeviceVlan> DeviceVlans { get; set; }

    public virtual DbSet<FirmwareStandard> FirmwareStandards { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<NetworkConfig> NetworkConfigs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Vlan> Vlans { get; set; }

    public virtual DbSet<Port> Ports { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5864DB56D7F");

            entity.HasIndex(e => e.Username, "UQ__Accounts__536C85E4EEC4D743").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<ActionLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__ActionLo__5E5499A84ED10020");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.LogTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.ActionLogs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Account");

            entity.HasOne(d => d.Device).WithMany(p => p.ActionLogs)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("FK_Log_Device");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("PK__Devices__49E12331B8903405");

            entity.HasIndex(e => e.SerialNumber, "UQ__Devices__048A00086CE5DED3").IsUnique();

            entity.HasIndex(e => e.Macaddress, "UQ__Devices__95BFB049C362CDEF").IsUnique();

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeviceName).HasMaxLength(100);
            entity.Property(e => e.DeviceTypeId).HasColumnName("DeviceTypeID");
            entity.Property(e => e.FirmwareVersion).HasMaxLength(50);
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Macaddress)
                .HasMaxLength(17)
                .HasColumnName("MACAddress");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Offline");

            entity.HasOne(d => d.DeviceType).WithMany(p => p.Devices)
                .HasForeignKey(d => d.DeviceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Device_Type");

            entity.HasOne(d => d.Location).WithMany(p => p.Devices)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Device_Location");
        });

        modelBuilder.Entity<DeviceType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__DeviceTy__516F039527279123");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<DeviceVlan>(entity =>
        {
            entity.HasKey(e => new { e.DeviceId, e.Vlanid }).HasName("PK__DeviceVL__6ED170E987247EE8");

            entity.ToTable("DeviceVLANs");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.Vlanid).HasColumnName("VLANID");
            entity.Property(e => e.AssignmentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Device).WithMany(p => p.DeviceVlans)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("FK_DV_Device");

            entity.HasOne(d => d.Vlan).WithMany(p => p.DeviceVlans)
                .HasForeignKey(d => d.Vlanid)
                .HasConstraintName("FK_DV_VLAN");
        });

        modelBuilder.Entity<FirmwareStandard>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Firmware__516F03958B0EF1D6");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("TypeID");
            entity.Property(e => e.LatestVersion).HasMaxLength(50);
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

            entity.HasOne(d => d.Type).WithOne(p => p.FirmwareStandard)
                .HasForeignKey<FirmwareStandard>(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Std_Type");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA4776BB8C99F");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Building).HasMaxLength(100);
            entity.Property(e => e.Floor).HasMaxLength(50);
            entity.Property(e => e.Rack).HasMaxLength(50);
            entity.Property(e => e.Room).HasMaxLength(50);
        });

        modelBuilder.Entity<NetworkConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__NetworkC__C3BC333C35CC4D54");

            entity.HasIndex(e => e.DeviceId, "UQ__NetworkC__49E123305F253B55").IsUnique();

            entity.Property(e => e.ConfigId).HasColumnName("ConfigID");
            entity.Property(e => e.DefaultGateway).HasMaxLength(45);
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .HasColumnName("IPAddress");
            entity.Property(e => e.PrimaryDns)
                .HasMaxLength(45)
                .HasColumnName("PrimaryDNS");
            entity.Property(e => e.SecondaryDns)
                .HasMaxLength(45)
                .HasColumnName("SecondaryDNS");
            entity.Property(e => e.SubnetMask).HasMaxLength(45);
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Device).WithOne(p => p.NetworkConfig)
                .HasForeignKey<NetworkConfig>(d => d.DeviceId)
                .HasConstraintName("FK_Config_Device");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AA3AEB444");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160DAEE1EC8").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Vlan>(entity =>
        {
            entity.HasKey(e => e.Vlanid).HasName("PK__VLANs__73053D83342B612F");

            entity.ToTable("VLANs");

            entity.Property(e => e.Vlanid)
                .ValueGeneratedNever()
                .HasColumnName("VLANID");
            entity.Property(e => e.GatewayIp)
                .HasMaxLength(45)
                .HasColumnName("GatewayIP");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.SubnetMask).HasMaxLength(50);
            entity.Property(e => e.Vlanname)
                .HasMaxLength(50)
                .HasColumnName("VLANName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

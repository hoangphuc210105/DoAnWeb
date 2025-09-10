using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DoAnWeb.Models
{
    public partial class QLDTContext : DbContext
    {
        public QLDTContext()
        {
        }

        public QLDTContext(DbContextOptions<QLDTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ctdonhang> Ctdonhangs { get; set; } = null!;
        public virtual DbSet<Dongsanpham> Dongsanphams { get; set; } = null!;
        public virtual DbSet<Donhang> Donhangs { get; set; } = null!;
        public virtual DbSet<Giohang> Giohangs { get; set; } = null!;
        public virtual DbSet<Khachhang> Khachhangs { get; set; } = null!;
        public virtual DbSet<Nguoiquantri> Nguoiquantris { get; set; } = null!;
        public virtual DbSet<Sanpham> Sanphams { get; set; } = null!;
        public virtual DbSet<Trangthaidonhang> Trangthaidonhangs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LEGION;Initial Catalog=QLDT;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ctdonhang>(entity =>
            {
                entity.HasKey(e => new { e.Madonhang, e.Masp })
                    .HasName("PK_CTDH");

                entity.ToTable("CTDONHANG");

                entity.Property(e => e.Madonhang).HasColumnName("MADONHANG");

                entity.Property(e => e.Masp).HasColumnName("MASP");

                entity.Property(e => e.Gia)
                    .HasColumnType("money")
                    .HasColumnName("GIA");

                entity.Property(e => e.Soluongsp).HasColumnName("SOLUONGSP");

                entity.HasOne(d => d.MadonhangNavigation)
                    .WithMany(p => p.Ctdonhangs)
                    .HasForeignKey(d => d.Madonhang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CTDH_DH");

                entity.HasOne(d => d.MaspNavigation)
                    .WithMany(p => p.Ctdonhangs)
                    .HasForeignKey(d => d.Masp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CTDH_SP");
            });

            modelBuilder.Entity<Dongsanpham>(entity =>
            {
                entity.HasKey(e => e.Madongsp)
                    .HasName("PK_DSP");

                entity.ToTable("DONGSANPHAM");

                entity.Property(e => e.Madongsp).HasColumnName("MADONGSP");

                entity.Property(e => e.Tendong)
                    .HasMaxLength(100)
                    .HasColumnName("TENDONG");
            });

            modelBuilder.Entity<Donhang>(entity =>
            {
                entity.HasKey(e => e.Madonhang)
                    .HasName("PK_DH");

                entity.ToTable("DONHANG");

                entity.Property(e => e.Madonhang).HasColumnName("MADONHANG");

                entity.Property(e => e.Diachinhan).HasMaxLength(100);

                entity.Property(e => e.Dienthoainhan)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Htgiaohang)
                    .HasColumnName("HTGiaohang")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Htthanhtoan)
                    .HasColumnName("HTThanhtoan")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Makh).HasColumnName("MAKH");

                entity.Property(e => e.Matt)
                    .HasColumnName("MATT")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Ngaydat)
                    .HasColumnType("datetime")
                    .HasColumnName("NGAYDAT");

                entity.Property(e => e.Tennguoinhan).HasMaxLength(50);

                entity.Property(e => e.TriGia).HasColumnType("money");

                entity.HasOne(d => d.MakhNavigation)
                    .WithMany(p => p.Donhangs)
                    .HasForeignKey(d => d.Makh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DH_KH");

                entity.HasOne(d => d.MattNavigation)
                    .WithMany(p => p.Donhangs)
                    .HasForeignKey(d => d.Matt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DH_TT");
            });

            modelBuilder.Entity<Giohang>(entity =>
            {
                entity.HasKey(e => e.Magiohang)
                    .HasName("PK__GIOHANG__559F5534F1020980");

                entity.ToTable("GIOHANG");

                entity.HasIndex(e => new { e.Makh, e.Masp }, "UQ_GIOHANG")
                    .IsUnique();

                entity.Property(e => e.Magiohang).HasColumnName("MAGIOHANG");

                entity.Property(e => e.Makh).HasColumnName("MAKH");

                entity.Property(e => e.Masp).HasColumnName("MASP");

                entity.Property(e => e.Soluong)
                    .HasColumnName("SOLUONG")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MakhNavigation)
                    .WithMany(p => p.Giohangs)
                    .HasForeignKey(d => d.Makh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GH_KH");

                entity.HasOne(d => d.MaspNavigation)
                    .WithMany(p => p.Giohangs)
                    .HasForeignKey(d => d.Masp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GH_SP");
            });

            modelBuilder.Entity<Khachhang>(entity =>
            {
                entity.HasKey(e => e.Makh)
                    .HasName("PK__KHACHHAN__603F592C5954F231");

                entity.ToTable("KHACHHANG");

                entity.HasIndex(e => e.Email, "UQ__KHACHHAN__161CF724960AF985")
                    .IsUnique();

                entity.Property(e => e.Makh).HasColumnName("MAKH");

                entity.Property(e => e.Diachi)
                    .HasMaxLength(100)
                    .HasColumnName("DIACHI");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Gioitinh).HasColumnName("GIOITINH");

                entity.Property(e => e.Matkhau)
                    .HasMaxLength(100)
                    .HasColumnName("MATKHAU");

                entity.Property(e => e.Ngaysinh)
                    .HasColumnType("date")
                    .HasColumnName("NGAYSINH");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("SDT");

                entity.Property(e => e.Tendn)
                    .HasMaxLength(30)
                    .HasColumnName("TENDN");

                entity.Property(e => e.Tenkh)
                    .HasMaxLength(50)
                    .HasColumnName("TENKH");
            });

            modelBuilder.Entity<Nguoiquantri>(entity =>
            {
                entity.HasKey(e => e.UserAdmin)
                    .HasName("PK__NGUOIQUA__AF86462B309F1576");

                entity.ToTable("NGUOIQUANTRI");

                entity.Property(e => e.UserAdmin).HasMaxLength(30);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.PassAdmin).HasMaxLength(100);

                entity.Property(e => e.VaiTro).HasMaxLength(30);
            });

            modelBuilder.Entity<Sanpham>(entity =>
            {
                entity.HasKey(e => e.Masp)
                    .HasName("PK_SP");

                entity.ToTable("SANPHAM");

                entity.Property(e => e.Masp).HasColumnName("MASP");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .HasColumnName("COLOR");

                entity.Property(e => e.Gia)
                    .HasColumnType("money")
                    .HasColumnName("GIA");

                entity.Property(e => e.Giamgia)
                    .HasColumnType("money")
                    .HasColumnName("GIAMGIA");

                entity.Property(e => e.Hinhanh)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("HINHANH");

                entity.Property(e => e.Madongsp).HasColumnName("MADONGSP");

                entity.Property(e => e.Mota)
                    .HasMaxLength(4000)
                    .HasColumnName("MOTA");

                entity.Property(e => e.Ngaysanxuat)
                    .HasColumnType("date")
                    .HasColumnName("NGAYSANXUAT");

                entity.Property(e => e.Soluong).HasColumnName("SOLUONG");

                entity.Property(e => e.Tensp)
                    .HasMaxLength(50)
                    .HasColumnName("TENSP");

                entity.HasOne(d => d.MadongspNavigation)
                    .WithMany(p => p.Sanphams)
                    .HasForeignKey(d => d.Madongsp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SP_DSP");
            });

            modelBuilder.Entity<Trangthaidonhang>(entity =>
            {
                entity.HasKey(e => e.Matt)
                    .HasName("PK__TRANGTHA__6023720FC32D0666");

                entity.ToTable("TRANGTHAIDONHANG");

                entity.Property(e => e.Matt).HasColumnName("MATT");

                entity.Property(e => e.Tentt)
                    .HasMaxLength(50)
                    .HasColumnName("TENTT");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

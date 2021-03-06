// <auto-generated />
using System;
using IAM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IAM.Migrations.Tenant
{
    [DbContext(typeof(TenantContext))]
    partial class TenantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("IAM.Models.TenantInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("(UUID())");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id")
                        .HasName("pk_tenant");

                    b.ToTable("tenant");
                });
#pragma warning restore 612, 618
        }
    }
}

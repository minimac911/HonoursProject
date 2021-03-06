// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TenantManager.Data;

namespace TenantManager.Migrations
{
    [DbContext(typeof(TenantCustomizationContext))]
    partial class TenantCustomizationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("TenantManager.Models.CustomizationPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CodeSnippet")
                        .HasColumnType("longtext");

                    b.Property<string>("ControllerName")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("MethodName")
                        .HasColumnType("longtext");

                    b.HasKey("Id")
                        .HasName("tenant_manager_customization_point");

                    b.ToTable("tenant_manager_customization_point");
                });

            modelBuilder.Entity("TenantManager.Models.TenantCustomization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ControllerName")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MethodName")
                        .HasColumnType("longtext");

                    b.Property<string>("ServiceEndPoint")
                        .HasColumnType("longtext");

                    b.Property<string>("ServiceName")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("Id")
                        .HasName("tenant_manager_customization");

                    b.ToTable("tenant_manager_customization");
                });
#pragma warning restore 612, 618
        }
    }
}

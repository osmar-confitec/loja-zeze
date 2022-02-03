using BuildBlockCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data.Mappings
{
    public class AddressMapping : BaseMapping<Models.Address>
    {
        public override void Configure(EntityTypeBuilder<Models.Address> builder)
        {
            base.Configure(builder);



            builder.Property(c => c.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.ZipCode)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(c => c.Complement)
                .HasColumnType("varchar(250)");

            builder.Property(c => c.District)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Address");

            builder.HasOne(x => x.Customer)
              .WithOne(x => x.Address)
              .HasForeignKey<Models.Customer>(x => x.Id);

        }
    }
}

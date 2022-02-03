using BuildBlockCore.Data;
using BuildBlockCore.DomainObjects;
using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data.Mappings
{
    public class CustomerMapping : BaseMapping<Models.Customer>
    {
        public override void Configure(EntityTypeBuilder<Models.Customer> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name)
               .IsRequired()
               .HasColumnType("varchar(200)");

            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(CPF.CpfMaxLength)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({CPF.CpfMaxLength})");
            });


            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Mail)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EmailMaxLength})");
            });

            builder.HasOne(x => x.Address)
                .WithOne(x => x.Customer)
                .HasForeignKey<Address>(x => x.Id);

            builder.ToTable("Customers");

        }
    }
}

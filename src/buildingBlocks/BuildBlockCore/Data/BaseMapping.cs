using BuildBlockCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildBlockCore.Data
{

    public abstract class BaseMappingNoKey<T> : IEntityTypeConfiguration<T> where T : EntityDataBase
    {

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {


            builder.Property(c => c.Active)
                    .HasColumnName("Active")
                    .IsRequired(true)
                    ;

            builder.Property(c => c.DateRegister)
                   .HasColumnName("DateRegister")
                   .IsRequired(true)
                   ;

            builder.Property(c => c.DateUpdate)
                    .HasColumnName("DateUpdate")
                    .IsRequired(false)
                    ;

            builder.Property(c => c.UserInsertedId)
                  .HasColumnName("UserInsertedId")
                  .IsRequired(true)
                  ;

            builder.Property(c => c.UserUpdatedId)
                   .HasColumnName("UserUpdatedId")
                   .IsRequired(false)
                   ;

            builder.Property(c => c.DeleteDate)
                   .HasColumnName("DeleteDate")
                   .IsRequired(false)
                   ;
            builder.Property(c => c.UserDeletedId)
                    .HasColumnName("UserDeletedId")
                    .IsRequired(false)
                    ;




        }
    }
    public abstract class BaseMapping<T> : BaseMappingNoKey<T>, IEntityTypeConfiguration<T> where T : EntityDataBase
    {

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasKey(c => c.Id);
        }
    }
}

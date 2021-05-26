using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relikt_2_DataAccess.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.OrderHeader)
                .WithMany(x => x.OrderDetail)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.OrderDetail)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.SizeType)
                .WithMany(x => x.OrderDetail);
                //.IsRequired()
                //           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.OrderDetail);
            //.IsRequired()
            //           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ApplicationType)
                .WithMany(x => x.OrderDetail);
                 //.IsRequired()
                 //          .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.RightLeft)
                .WithMany(x => x.OrderDetail);
            //.IsRequired();
            //      .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.ColourOfGlass) 
                .WithMany(x => x.OrderDetail);
                //.IsRequired();
                  //        .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

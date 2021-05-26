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
    public class InquiryDetailConfiguration : IEntityTypeConfiguration<InquiryDetail>
    {
        public void Configure(EntityTypeBuilder<InquiryDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.InquiryHeader)
                .WithMany(x => x.InquiryDetail)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.InquiryDetail)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.SizeType)
                .WithMany(x => x.InquiryDetail)
                .IsRequired();
            //               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.InquiryDetail)
                .IsRequired();
            //               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApplicationType)
                .WithMany(x => x.InquiryDetail)
                .IsRequired();
            //               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RightLeft)
                .WithMany(x => x.InquiryDetail);
            //.IsRequired();
            //      .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.ColourOfGlass)
                .WithMany(x => x.InquiryDetail);
                //.IsRequired();
                  //        .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

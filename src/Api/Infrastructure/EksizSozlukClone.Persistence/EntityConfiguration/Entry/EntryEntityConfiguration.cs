using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksizSozlukClone.Infrastructure.Persistence.EntityConfiguration.Entry
{
    public class EntryEntityConfiguration: BaseEntityConfiguration<EksiSozlukClone.Core.Domain.Models.Entry>
    {
        public override void Configure(EntityTypeBuilder<EksiSozlukClone.Core.Domain.Models.Entry> builder)
        {
            base.Configure(builder);

            builder.HasOne(i => i.CreatedBy)
                .WithMany(i => i.Entries)
                .HasForeignKey(i => i.CreateById);  
        }
    }
}

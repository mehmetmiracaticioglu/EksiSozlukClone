﻿using EksiSozlukClone.Core.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksizSozlukClone.Infrastructure.Persistence.EntityConfiguration.Entry
{
    public class EntryFavroriteEntityConfiguration : BaseEntityConfiguration<EntryFavorite>
    {
        public override void Configure(EntityTypeBuilder<EntryFavorite> builder)
        {
            base.Configure(builder);

            builder.HasOne(i => i.Entry)
                .WithMany(i => i.EntryFavorites)
                .HasForeignKey(i => i.EntryId);

            builder.HasOne(i => i.CreatedUser)
                .WithMany(i => i.EntryFavorites)
                .HasForeignKey(i => i.CreatedById);
        }
    }
}
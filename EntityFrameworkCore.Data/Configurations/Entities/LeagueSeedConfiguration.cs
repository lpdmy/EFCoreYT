using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Data.Configurations.Entities
{
    public class LeagueSeedConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.HasData(
                new League
                {
                    Id = 20,
                    Name = "Dieu My - Sample League 1",
                    
                },
                new League
                {
                    Id = 21,
                    Name = "Dieu My 1 - Sample League 2",
                    
                },
                new League
                {
                    Id = 22,
                    Name = "Dieu My 2 - Sample League 3",
                    
                }
            );
        }
    }
}

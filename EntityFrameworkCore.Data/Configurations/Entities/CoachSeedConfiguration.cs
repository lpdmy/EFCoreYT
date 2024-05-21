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
    public class CoachSeedConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.HasData(
                new Coach
                {
                    Id = 20,
                    Name = "Dieu My",
                    TeamId = 20
                },
                new Coach
                {
                    Id = 21,
                    Name = "Dieu My 1",
                    TeamId = 21
                },
                new Coach
                {
                    Id = 22,
                    Name = "Dieu My 2",
                    TeamId = 22
                }
            );
        }
    }
}

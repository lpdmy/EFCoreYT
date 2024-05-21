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
    public class TeamSeedConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(
                new Team
                {
                    Id = 20,
                    Name = "Dieu My - Sample Team 1",
                    LeagueId = 20,
                },
                new Team
                {
                    Id = 21,
                    Name = "Dieu My 1 - Sample Team 2",
                    LeagueId = 21
                },
                new Team
                {
                    Id = 22,
                    Name = "Dieu My 2 - Sample Team 3",
                    LeagueId = 22
                }
            );
        }
    }
}

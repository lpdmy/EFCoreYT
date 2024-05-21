using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreYT
{
    public class Program
    {
        private static readonly FootballLeagueDbContext context = new FootballLeagueDbContext();

        static async Task Main(string[] args)
        {
            await AddNewCoach();
            Console.WriteLine("Press any key to End...");
            Console.ReadKey();
        }

       
        private static async Task RawSqlQuery()
        {
            string name = "MU";
            //var teams1 = await context.Teams.FromSqlRaw($"SELECT * FROM Teams WHERE Name = '{name}'").Include(p => p.League).ToListAsync();
            var teams2 = await context.Teams.FromSqlInterpolated($"SELECT * FROM TEAMS WHERE Name = {name}").ToListAsync();
        }
        private static async Task FilteringWithRelatedData()
        {
            var leagues = await context.Leagues.Where(q => q.Teams.Any(x => x.Name.Contains("M"))).Include(q => q.Teams).ToListAsync();
        }

        private static async Task QueryRelatedRecords()
        {
            //Get Many Related Records - Leagues -> Teams66
            var leauges = await context.Leagues.Include(league => league.Teams).ToListAsync();
            //var leagues = await context.Leagues.Include(q => q.Teams).ToListAsync();

            //Get One Related Record - Team -> Coach

            //var team = await context.Teams.Include(q => q.Coach).FirstOrDefaultAsync(q => q.Id == 2);
            //Get "Grand Children" Related Record - Team -> Matches -> Home/ Away Team
            var teams = await context.Teams.Include(p => p.HomeMatches).ThenInclude(p => p.AwayTeam).FirstOrDefaultAsync(p => p.Id == 2);

            //Get One Record with Related Record(s) - Team -> Matches -> Home/ Away Team
            //Get Includes with filters
        }
        //1-1
        private static async Task AddNewCoach()
        {
            await context.AddAsync(new Coach() { Name = "Conan", TeamId = 3});
            await context.SaveChangesAsync();
        }
        private static async Task AddNewMatches()
        {
            List<Match> matches = new List<Match>() {
                new Match() { AwayTeamId = 1, HomeTeamId = 2, Date = new DateTime(2021, 12, 28)},
                new Match() { AwayTeamId = 2, HomeTeamId = 3, Date = DateTime.Now},
                new Match() { HomeTeamId = 4, AwayTeamId = 3, Date = new DateTime(2022, 12, 23)}
            };
            await context.Matches.AddRangeAsync(matches);
            await context.SaveChangesAsync();
        }
        private static async Task AddNewLeagueWithTeams()
        {
            List<Team> teams = new List<Team>() { 
                new Team() { Name = "T1" },
                new Team() { Name = "GenG" }
            };
            await context.Leagues.AddAsync(new League { Name = "FPT", Teams = teams });
            await context.SaveChangesAsync();
        }
        private static async Task AddNewTeamWithLeagueId ()
        {
            await context.Teams.AddAsync(new Team
            {
                Name = "Arsenal",
                LeagueId = 2
            });
            await context.SaveChangesAsync();
        }


        private static async Task TrackingVsNoTracking()
        {

            //ko lưu vào bộ nhớ đệm khi theo dõi các Entity, ko theo dõi nên ko thay đổi trong database
            var withNoTracking = await context.Leagues.AsNoTracking().FirstOrDefaultAsync(l => l.Id == 02);

            withNoTracking.Name = "Updated League";
            await context.SaveChangesAsync();


        }


        private static async Task DeleteWithRelative()
        {
            League league = await context.Leagues.FindAsync(5);
            context.Remove(league);
            await context.SaveChangesAsync();
        }
        private static async Task SimpleDelete()
        {
            League league = await context.Leagues.FindAsync(1);
            context.Remove(league);
            await context.SaveChangesAsync();
        }
        private static async Task GetRecord()
        {
            var league = await context.Leagues.FindAsync(7);
            Console.WriteLine($"{league.Id} - {league.Name}");
        }

        private static async Task SimpleUpdateTeamRecord()
        {
            var team = new Team
            {
                Id = 5,
                Name = "Manchester City",
                LeagueId = 5
            };

            context.Teams.Update(team);
            await context.SaveChangesAsync();
        }
        private static async Task UpdateRecord()
        {
            //Retrieve Record
            var league = await context.Leagues.FindAsync(7);

            //Make Changes
            league.Name = "Scottish Premiership";

            //Save Changes
            await context.SaveChangesAsync();

            GetRecord();
        }
        private static async Task AlternativeLinq()
        {
            Console.Write($"Enter name of team: ");
            var name = Console.ReadLine();
            var teams = await (from t in context.Teams
                               where EF.Functions.Like(t.Name, $"%{name}%")
                               select t)
                        .ToListAsync();
            foreach (var team in teams)
            {
                Console.WriteLine($"{team.Id} - {team.Name}");
            }
        }
        private static async Task AdditionalExecuteQuery()
        {
            var league = await context.Leagues.FindAsync(1);
            Console.WriteLine(league.Name);
            var leagueFirst = await context.Leagues.SingleOrDefaultAsync(p => p.Name == "Lu Liga");
            Console.WriteLine($"{leagueFirst}");
        }
        private static async Task AdditionalQuery()
        {
            Console.WriteLine("Enter League Name (Or Part Of): ");
            var name = Console.ReadLine();
            var leagues = await context.Leagues.Where(l => EF.Functions.Like(l.Name, "%" + name + "%")).ToListAsync();
            foreach (var l in leagues)
            {
                Console.WriteLine($"{l.Id} - {l.Name}");
            }
        }
        private static async Task FilterQuery()
        {
            Console.WriteLine("Enter League Name (Or Part Of): ");
            var name = Console.ReadLine();
            var exactMatches = await context.Leagues.Where(l => l.Name.Equals(name)).ToListAsync();
            Console.WriteLine("Exact Matches Leagues: ");
            foreach (var league in exactMatches)
            {
                Console.WriteLine($"{league.Id} - {league.Name}");
            };
            var partialMatches = await context.Leagues.Where(l => EF.Functions.Like("%My")).ToListAsync(); // Like la 1 lenh trong SQL dung de so sanh 2 chuoi 
            // str like DieuMy => dieumy, Dieumy, DIeuMy => true
            // like %My => "Dieu My" "Tra My" => true
            // like %My% => "DHFHDMydjfh" => true
            Console.WriteLine("Partial Matches Leagues: ");
            foreach (var league in partialMatches)
            {
                Console.WriteLine($"{league.Id} - {league.Name}");
            }

        }

        static async Task AddNewTeamWithLeague()
        {
            League league = new League()
            {
                Name = "La liga",
            };

            Team team = new Team()
            {
                Name = "Barcelona",
                League = league
            };

            await context.AddAsync(team);

        }

        static async Task AddTeamsWithLeague(League league)
        {
            var teams = new List<Team>()
                {
                    new Team
                    {
                        Name = "Juventus",
                        LeagueId = league.Id,
                    },
                    new Team
                    {
                        Name = "AC Milan",
                        LeagueId = league.Id,
                    },
                    new Team
                    {
                        Name = "AS Roman",
                        League = league,
                    }
                };

            await context.Teams.AddRangeAsync(teams);
        }

        static async Task SimpleSelectQuery()
        {
            var leagues = await context.Leagues.ToListAsync();
            foreach (var league in leagues)
            {
                Console.WriteLine($"{league.Id} - {league.Name}");
            }
        }
    }
}


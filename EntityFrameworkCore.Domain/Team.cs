using EntityFrameworkCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Domain
{
    public class Team : BaseDomainObject
    {
        public string Name { get; set; }
       
        //Foreign Key
        //Because of naming convention, even if there is no virtual League variable,
        //EF Core still know LeagueId is a foreign key
        public int LeagueId {  get; set; }

        public virtual League League { get; set; }
        public virtual Coach Coach { get; set; }
        public virtual List<Match> HomeMatches { get; set; }

        public virtual List<Match> AwayMatches { get; set; }
    }
}

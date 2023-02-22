using EksiSozlukClone.Core.Application.Interface.Repositories;
using EksiSozlukClone.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksizSozlukClone.Infrastructure.Persistence.Repositories
{
    public class EntryVoteRepository : GenericRepository<EntryVote>, IEntryVoteRepository
    {
        public EntryVoteRepository(EksiSozlukCloneContext dbContex) : base(dbContex)
        {
        }
    }
}

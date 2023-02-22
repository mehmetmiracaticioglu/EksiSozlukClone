using EksiSozlukClone.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Core.Domain.Models;

public class EntryVote:BaseEntity
{
    public Guid EntryId { get; set; }   
    public Guid CreatedById { get; set; }   
    public VoteType voteType { get; set; }  

    public virtual Entry Entry { get; set; }


}

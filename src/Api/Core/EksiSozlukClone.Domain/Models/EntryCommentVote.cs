using EksiSozlukClone.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Core.Domain.Models;

public class EntryCommentVote : BaseEntity
{
    public Guid EntryCommentId { get; set; }   
    public Guid CreatedById { get; set; }   
    public VoteType voteType { get; set; }  

    public virtual EntryComment EntryComment { get; set; }


}

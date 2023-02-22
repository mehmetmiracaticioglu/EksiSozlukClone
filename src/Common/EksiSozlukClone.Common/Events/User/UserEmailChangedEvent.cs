using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Common.Events.User
{
    public class UserEmailChangedEvent
    {
        public string oldEmailAdress { get; set; }
        public string newEmailAdress { get; set; }
    }
}

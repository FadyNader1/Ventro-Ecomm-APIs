using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.EmailSetting
{
    public class EmailConfig
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string EmailSender { get; set; }
        public string EmailPassword { get; set; }

    }
}

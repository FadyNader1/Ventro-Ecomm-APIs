using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IEmailSetting
    {
        public Task SendEmailAsync(string sub, string body, string to);
    }
}

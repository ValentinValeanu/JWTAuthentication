using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Services.Models
{
    public class RefreshTokenOutput
    {
        public required string AccessToken { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Contracts.Requests
{
    public class TokenGenerationRequest
    {
        public required Guid UserId { get; set; }

        public required string Email { get; set; }

        public Dictionary<string, object> CustomClaims { get; set; } = new();
    }
}

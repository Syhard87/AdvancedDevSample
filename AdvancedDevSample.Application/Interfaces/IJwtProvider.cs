using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Interfaces
{
    public interface IJwtProvider
    {
        // Cette méthode fabriquera la longue chaîne de caractères (le token)
        string GenerateToken(Guid userId, string email);
    }
}
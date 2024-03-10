using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badil.Backend.Services
{
    public interface IOpenAiService
    {
        Task<string> GenerateSearchTerm(string brandName, string productName);
    }
}

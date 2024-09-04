using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Domain.Entities
{
    public class BaseResult<T>
    {
        public bool isSuccess { get; set; } = true;
        public T? data { get; set; }
        public string? errorMessage { get; set; }
    }
}

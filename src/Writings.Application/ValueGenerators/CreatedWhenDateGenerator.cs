using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Application.ValueGenerators
{
    public class CreatedWhenDateGenerator : ValueGenerator<DateTimeOffset>
    {
        public override bool GeneratesTemporaryValues => false;

        public override DateTimeOffset Next(EntityEntry entry)
        {
            return DateTimeOffset.Now;
        }
    }
}

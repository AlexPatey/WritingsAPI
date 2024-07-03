using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Api.Tests.Integration
{
    [CollectionDefinition("WritingsApi Collection")]
    public class TestCollection : ICollectionFixture<WritingsApiFactory>
    {
    }
}

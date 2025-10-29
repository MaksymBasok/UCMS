using Tests.Common;
using Xunit;

namespace Api.Tests.Integration;

[CollectionDefinition("Integration", DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebFactory>
{
}

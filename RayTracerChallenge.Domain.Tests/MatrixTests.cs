namespace RayTracerChallenge.Domain.Tests;

public class MatrixTests
{
    private readonly IFixture _fixture = new Fixture();

    [Theory]
    [InlineData(4, 4)]
    [InlineData(3, 3)]
    [InlineData(2, 2)]
    public void ConstructMatrix(int width, int height)
    {
        var data = CreateTestData(width, height);
        var matrix = new Matrix(data);

        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                matrix[y, x] = data[y, x];
            }
        }

        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                Assert.Equal(data[y, x], matrix[y, x]);
            }
        }
    }

    private float[,] CreateTestData(int width, int height)
    {
        var data = new float[width, height];
        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                data[y, x] = _fixture.Create<float>();
            }
        }

        return data;
    }
}

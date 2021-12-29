namespace RayTracerChallenge.Domain;

public class Material
{
    public Color Color { get; init; } = new Color
    {
        Red = 1,
        Green = 1,
        Blue = 1
    };

    public decimal Ambient { get; init; } = 0.1M;
    public decimal Diffuse { get; init; } = 0.9M;
    public decimal Specular { get; init; } = 0.9M;
    public decimal Shininess { get; init; } = 200.0M;

    public Color Lighting(Tuple point, Light light, Tuple eye, Tuple normal, bool inShadow = false)
    {
        if(!point.IsPoint)
        {
            throw new ArgumentException($"{nameof(point)} must be a point");
        }
        if(!eye.IsVector)
        {
            throw new ArgumentException($"{nameof(eye)} must be a vector");
        }
        if(!normal.IsVector)
        {
            throw new ArgumentException($"{nameof(normal)} must be a vector");
        }

        // combine the surface color with the light's color and intensity
        var effectiveColor = Color * light.Intensity;

        // find the direction of the light source
        var lightVector = (light.Position - point).Normalize();

        // compute the ambient contribution
        var ambient = effectiveColor * Ambient;

        Color diffuse;
        Color specular;

        // lightDotNormal represnets the cosine of the angle between the
        // light vector and the normal vector. A negative number means the
        // light is on the other side of the surface.
        var lightDotNormal = lightVector.DotProduct(normal);
        if(lightDotNormal < 0 || inShadow)
        {
            diffuse = Color.Black;
            specular = Color.Black;
        }
        else
        {
            // compute the diffuse contribution
            diffuse = effectiveColor * Diffuse * lightDotNormal;

            // reflectDotEye represents the cosine of the anglet between the
            // reflection vector and the eye vector. A negative number means the
            // light reflects away from the eye.
            var reflect = (lightVector * -1).Reflect(normal);
            var reflectDotEye = reflect.DotProduct(eye);

            if(reflectDotEye <= 0)
            {
                specular = Color.Black;
            }
            else
            {
                // compute the specular contribution
                var factor = Convert.ToDecimal(Math.Pow(Convert.ToDouble(reflectDotEye), Convert.ToDouble(Shininess)));
                specular = light.Intensity * Specular * factor;
            }
        }

        // add the three contributions together to get the final shading
        return ambient + diffuse + specular;
    }
}

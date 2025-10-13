namespace RayTracerChallenge.Domain;

public record Material
{
    public Color Color { get; init; }
    public decimal Ambient { get; init; }
    public decimal Diffuse { get; init; }
    public decimal Specular { get; init; }
    public decimal Shininess { get; init; }

    public Material(Color? color = null,
        decimal? ambient = null,
        decimal? diffuse = null,
        decimal? specular = null,
        decimal? shininess = null)
    {
        Color = color ?? new Color(1, 1, 1);
        Ambient = ambient ?? 0.1M;
        Diffuse = diffuse ?? 0.9M;
        Specular = specular ?? 0.9M;
        Shininess = shininess ?? 200;
    }

    public Color Lighting(PointLight light,
        Tuple position,
        Tuple eyeVector,
        Tuple normalVector)
    {
        // Combine surface color with light's color and intensity
        var effectiveColor = Color * light.Intensity;
        
        // Find the direction to the light source
        var lightVector = (light.Position - position).Normalize();
        
        // Compute the ambient contribution
        var ambient = effectiveColor * Ambient;

        Color diffuse, specular;
        
        // lightDotNormal represents the cosine of the angle between the
        // light vector and the normal vector. A negative number means the
        // light is on the other side of the surface.
        var lightDotNormal = lightVector.Dot(normalVector);
        if (lightDotNormal < 0)
        {
            diffuse = new Color(0, 0, 0);
            specular = new Color(0, 0, 0);
        }
        else
        {
            // Compute the diffuse contribution
            diffuse = effectiveColor * Diffuse * lightDotNormal;
            
            // relfectDotEye represents the cosine of the angle between the
            // reflection vector and the eye vector. A negative number means the
            // light reflects away from the eye.
            var reflectVector = (-lightVector).Reflect(normalVector);
            var reflectDotEye = reflectVector.Dot(eyeVector);

            if (reflectDotEye <= 0)
            {
                specular = new Color(0, 0, 0);
            }
            else
            {
                // Compute the specular contribution
                var factor = (decimal)Math.Pow((double)reflectDotEye, (double)Shininess);
                specular = light.Intensity * Specular * factor;
            }
        }
        
        // Add the three contributions together to get the final shading
        return ambient + diffuse + specular;
    }
};
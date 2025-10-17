using RayTracerChallenge.Domain.Patterns;
using RayTracerChallenge.Domain.Shapes;

namespace RayTracerChallenge.Domain;

public record Material
{
    public Color Color { get; init; }
    public double Ambient { get; init; }
    public double Diffuse { get; init; }
    public double Specular { get; init; }
    public double Shininess { get; init; }
    public double Reflective { get; init; }
    public double Transparency { get; init; }
    public double RefractiveIndex { get; init; }
    public Pattern? Pattern { get; init; }

    public Material(Color? color = null,
        double? ambient = null,
        double? diffuse = null,
        double? specular = null,
        double? shininess = null,
        double? reflective = null,
        double? transparency = null,
        double? refractiveIndex = null,
        Pattern? pattern = null)
    {
        Color = color ?? new Color(1, 1, 1);
        Ambient = ambient ?? 0.1;
        Diffuse = diffuse ?? 0.9;
        Specular = specular ?? 0.9;
        Shininess = shininess ?? 200;
        Reflective = reflective ?? 0;
        Transparency = transparency ?? 0;
        RefractiveIndex = refractiveIndex ?? 1;
        Pattern = pattern;
    }

    public Color Lighting(Shape shape,
        PointLight light,
        Tuple position,
        Tuple eyeVector,
        Tuple normalVector,
        bool inShadow = false)
    {
        var color = Pattern?.ColorAtForObject(shape, position) ?? Color;
        
        // Combine surface color with light's color and intensity
        var effectiveColor = color * light.Intensity;
        
        // Find the direction to the light source
        var lightVector = (light.Position - position).Normalize();
        
        // Compute the ambient contribution
        var ambient = effectiveColor * Ambient;

        if (inShadow)
        {
            return ambient;
        }

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
                var factor = Math.Pow(reflectDotEye, Shininess);
                specular = light.Intensity * Specular * factor;
            }
        }
        
        // Add the three contributions together to get the final shading
        return ambient + diffuse + specular;
    }
}
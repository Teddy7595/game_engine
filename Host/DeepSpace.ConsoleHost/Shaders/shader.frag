#version 330 core
out vec4 FragColor;

struct Material {
    vec3 diffuse;   
    float shininess;
};

uniform sampler2D textureSampler;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords; 

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 lightColor;
uniform Material material;

void main()
{
    
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
  	
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuseLight = diff * lightColor;
    
    float specularStrength = 1.0;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = specularStrength * spec * lightColor;
        
    vec3 textureColor = texture(textureSampler, TexCoords).rgb;
    
    vec3 diffuseColor = material.diffuse * textureColor;
    
    vec3 result = (ambient + diffuseLight + specular) * diffuseColor;
    FragColor = vec4(result, 1.0);
}
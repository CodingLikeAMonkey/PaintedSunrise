shader_type spatial;

// Albedo inputs (texture + color)
uniform sampler2D albedo_texture : hint_default_white;
uniform vec4 albedo_color : source_color = vec4(1.0);

// PBR inputs
uniform sampler2D roughness_texture : hint_default_white;
uniform sampler2D normal_texture : hint_normal;
uniform float roughness_adjust : hint_range(0.0, 1.0) = 1.0;
uniform float normal_strength : hint_range(-16.0, 16.0) = 1.0;

void fragment() {
	// Sample textures
	vec4 albedo_sample = texture(albedo_texture, UV);
	vec4 roughness_sample = texture(roughness_texture, UV);
	vec3 normal_sample = texture(normal_texture, UV).rgb;
	
	// Combine albedo texture with color
	vec3 final_albedo = albedo_sample.rgb * albedo_color.rgb;
	float final_alpha = albedo_sample.a * albedo_color.a;
	
	// Convert normal map from [0,1] to [-1,1] range
	vec3 tangent_normal = normalize(normal_sample * 2.0 - 1.0);
	
	// Set material properties
	ALBEDO = final_albedo;
	ALPHA = final_alpha;
	ROUGHNESS = roughness_sample.r * roughness_adjust;
	NORMAL_MAP = tangent_normal;
	NORMAL_MAP_DEPTH = normal_strength;
	
	// Set default PBR values
	METALLIC = 0.0;
	SPECULAR = 0.5;
	AO = 1.0;
	EMISSION = vec3(0.0);
}
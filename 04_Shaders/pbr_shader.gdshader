shader_type spatial;

uniform bool render_backfaces = false;

render_mode blend_mix, depth_draw_opaque, cull_disabled, diffuse_burley, specular_schlick_ggx;

uniform vec4 albedo : source_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform sampler2D texture_albedo : source_color, filter_linear_mipmap, repeat_enable;
uniform ivec2 albedo_texture_size;
uniform float point_size : hint_range(0.1, 128.0, 0.1) = 1.0;

uniform float roughness : hint_range(0.0, 1.0) = 1.0;
uniform sampler2D texture_metallic : hint_default_white, filter_linear_mipmap, repeat_enable;
uniform vec4 metallic_texture_channel;
uniform sampler2D texture_roughness : hint_roughness_r, filter_linear_mipmap, repeat_enable;

uniform float specular : hint_range(0.0, 1.0, 0.01) = 0.5;
uniform float metallic : hint_range(0.0, 1.0, 0.01);

uniform sampler2D texture_normal : hint_roughness_normal, filter_linear_mipmap, repeat_enable;
uniform float normal_scale : hint_range(-16.0, 16.0) = 1.0;

uniform vec3 uv1_scale = vec3(1.0, 1.0, 1.0);
uniform vec3 uv1_offset;
uniform vec3 uv2_scale = vec3(1.0, 1.0, 1.0);
uniform vec3 uv2_offset;
uniform float uv1_rotate_degrees : hint_range(-180.0, 180.0) = 0.0;

void vertex() {
	vec2 uv = UV * uv1_scale.xy + uv1_offset.xy;

	// Rotate UV around (0.5, 0.5)
	float angle = radians(uv1_rotate_degrees);
	float cos_a = cos(angle);
	float sin_a = sin(angle);

	uv -= vec2(0.5); // translate to origin
	uv = vec2(
		uv.x * cos_a - uv.y * sin_a,
		uv.x * sin_a + uv.y * cos_a
	);
	uv += vec2(0.5); // translate back

	UV = uv;
}

void fragment() {
	if (!render_backfaces && !FRONT_FACING) {
		discard;
	}

	vec2 base_uv = UV;

	vec4 albedo_tex = texture(texture_albedo, base_uv);
	ALBEDO = albedo.rgb * albedo_tex.rgb;

	float metallic_tex = dot(texture(texture_metallic, base_uv), metallic_texture_channel);
	METALLIC = metallic_tex * metallic;
	SPECULAR = specular;

	vec4 roughness_texture_channel = vec4(1.0, 0.0, 0.0, 0.0);
	float roughness_tex = dot(texture(texture_roughness, base_uv), roughness_texture_channel);
	ROUGHNESS = roughness_tex * roughness;

	NORMAL_MAP = texture(texture_normal, base_uv).rgb;
	NORMAL_MAP_DEPTH = normal_scale;
}

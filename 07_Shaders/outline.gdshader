shader_type spatial;
render_mode cull_front, unshaded, depth_draw_always;

// Toggle outline (1.0 = on, 0.0 = off)
uniform float outline_strength : hint_range(0.0, 1.0) = 0.0;

uniform float outline_thickness : hint_range(0.0, 0.05, 0.001) = 0.015;
uniform vec4 outline_color : source_color = vec4(0.2, 1.0, 1.0, 1.0);

void vertex() {
    // Multiply thickness by toggle to disable extrusion when off
    VERTEX += NORMAL * outline_thickness * outline_strength;
}

void fragment() {
    if (outline_strength < 0.5) {
        discard;
    }

    ALBEDO = outline_color.rgb;
    ALPHA = outline_color.a;
}

#version 320 es
/* Copyright (c) 2014-2017, ARM Limited and Contributors
 *
 * SPDX-License-Identifier: MIT
 *
 * Permission is hereby granted, free of charge,
 * to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

// Fragment shader
precision mediump float;

// Input uniforms to replace pixel local storage
uniform sampler2D lightingTexture;
uniform sampler2D depthTexture;    // For minMaxDepth
uniform sampler2D albedoTexture;
uniform sampler2D normalTexture;

// Output
layout(location = 0) out vec4 outLighting;

// Light properties (unchanged)
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float lightIntensity;
uniform float lightRadius;

// Scattering parameters (unchanged)
uniform float ambient;
uniform float distortion;
uniform float sharpness;
uniform float scale;

// Position reconstruction parameters (unchanged)
uniform float zNear;
uniform float zFar;
uniform float top;
uniform float right;
uniform vec2 invResolution;

// Subsurface scattering shading function (unchanged)
float sss(vec3 P, vec3 L, vec3 V, vec3 N, float r, float thickness)
{
    vec3 Lt = -(L + N * distortion);
    const float invln2 = 1.4427;
    float VdotL = exp2(-sharpness * invln2 * (1.0 - max(dot(V, Lt), 0.0))) * scale;
    return (VdotL + ambient) * clamp(1.0 - thickness * 0.5, 0.0, 1.0);
}

// Lighting function (unchanged)
vec3 lighting(vec3 P, vec3 Lp, vec3 N, vec3 V, float thickness, float Li, vec3 Ldiff, vec3 Cdiff)
{
    vec3 L = Lp - P;
    float r = length(L);
    L /= r;
    vec3 H = normalize(V + L);
    float specular = clamp(6.0 * exp2(-128.0 * (1.0 - max(dot(N, H), 0.0))), 0.0, 1.0);
    float NdotL = max(dot(N, L), 0.0);
    thickness = min(thickness, r);
    float Is = sss(P, L, V, N, r, thickness);
    float attenuation = 10.0 * lightRadius * Li / (1.0 + 0.1 * r * r);
    return attenuation * (Is + specular + NdotL) * Ldiff * Cdiff;
}

void main()
{
    vec2 uv = gl_FragCoord.xy * invResolution;

    // Read from textures instead of pixel local storage
    vec2 minMaxDepth = texture2D(depthTexture, uv).rg;
    vec4 albedoData = texture2D(albedoTexture, uv);
    vec2 normalXY = texture2D(normalTexture, uv).rg;
    vec4 currentLighting = texture2D(lightingTexture, uv);

    float depth = minMaxDepth.x;
    float depthNormalized = clamp((depth - zNear) / (zFar - zNear), 0.0, 1.0);

    // Reconstruct view-space position from depth (unchanged)
    vec2 fragCoordNorm = vec2(-1.0) + 2.0 * gl_FragCoord.xy * invResolution;
    vec2 frustum = vec2(right, top);
    vec3 P = vec3(fragCoordNorm * frustum * (1.0 + (zFar / zNear - 1.0) * depthNormalized), -depth);
    vec3 V = -normalize(P);

    // Reconstruct view-space normal
    vec3 N = vec3(normalXY, 0.0);
    float nzsign = albedoData.a;
    N.z = nzsign * sqrt(1.0 - min(dot(N.xy, N.xy), 1.0));

    // Additive lighting
    vec3 albedo = albedoData.rgb;
    float thickness = minMaxDepth.y - minMaxDepth.x;

    // Output lighting (additive blend with existing lighting)
    outLighting = currentLighting;
    outLighting.rgb += lighting(P, lightPos, N, V, thickness, lightIntensity, lightColor, albedo);
}
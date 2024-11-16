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
precision mediump float;

// Input textures from previous passes
uniform sampler2D lightingTexture;   // rgb10_a2 format
uniform sampler2D minMaxDepthTexture; // rg16f format
uniform sampler2D albedoTexture;      // rgb10_a2 format
uniform sampler2D normalXYTexture;    // rg16f format

uniform float zNear;
uniform float zFar;

in vec2 vTexCoord; // UV coordinates from vertex shader
out vec4 outColor;

void main()
{
    // Sample the lighting texture at the current fragment position
    vec4 lighting = texture(lightingTexture, vTexCoord);

    // Write accumulated lighting back to framebuffer
    // with gamma correction (gamma of 2.0)
    outColor.rgb = sqrt(lighting.rgb);
    // Alpha lighting is unused
    outColor.a = 1.0;
}
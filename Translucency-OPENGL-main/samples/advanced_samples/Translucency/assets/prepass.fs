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

precision highp float;

// Define layout locations to match the original storage structure order
layout(location = 0) out vec4 outLighting;    // rgb10_a2 format
layout(location = 1) out vec2 outMinMaxDepth; // rg16f format
layout(location = 2) out vec4 outAlbedo;     // rgb10_a2 format
layout(location = 3) out vec2 outNormalXY;   // rg16f format

uniform vec3 albedo;
in vec4 vPosition;
in vec3 vNormal;

void main()
{
    vec3 n = normalize(vNormal);
    outLighting = vec4(0.0);
    outMinMaxDepth = vec2(-vPosition.z, -vPosition.z);
    outAlbedo = vec4(albedo, sign(n.z));
    outNormalXY = n.xy;
}
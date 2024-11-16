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

// Output declarations to match the original pixel local storage structure
layout(location = 0) out vec4 outLighting;
layout(location = 1) out vec2 outMinMaxDepth;
layout(location = 2) out vec4 outAlbedo;
layout(location = 3) out vec2 outNormalXY;

// Original uniforms
uniform vec3 albedo;

// Original inputs
in vec4 vPosition;
in vec3 vNormal;

void main()
{
    float depth = -vPosition.z;

    // Load existing minMaxDepth.y value
    float currentMaxDepth = outMinMaxDepth.y;

    // Update only minMaxDepth.y while preserving other values
    outMinMaxDepth = vec2(outMinMaxDepth.x, max(depth, currentMaxDepth));

    // Keep other outputs unchanged to maintain their current values
    outLighting = outLighting;
    outAlbedo = outAlbedo;
    outNormalXY = outNormalXY;
}
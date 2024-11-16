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

uniform sampler2D depthTexture;    // Previous depth texture
in vec4 vPosition;
in vec3 vNormal;
in vec2 vTexCoord;

// Output updated depth information
layout(location = 0) out vec4 outDepth;

void main()
{
    // Get the current depth values from the depth texture
    vec4 currentDepth = texture(depthTexture, vTexCoord);
    
    // Calculate new depth
    float depth = -vPosition.z;
    
    // Keep the minimum depth from the original texture (x component)
    // and update the maximum depth (y component)
    outDepth = vec4(currentDepth.x, max(depth, currentDepth.y), 0.0, 1.0);
}

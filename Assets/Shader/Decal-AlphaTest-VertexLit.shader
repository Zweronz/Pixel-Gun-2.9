�Shader "Decal/Cutout VertexLit" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader { 
 Tags { "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  Lighting On
  AlphaToMask On
  Material {
   Ambient [_Color]
   Diffuse [_Color]
  }
  AlphaTest Greater [_Cutoff]
  ColorMask RGB
  Offset -1, -1
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
  }
  AlphaToMask On
  AlphaTest Greater [_Cutoff]
  ColorMask RGB
  Offset -1, -1
  SetTexture [unity_Lightmap] { Matrix [unity_LightmapMatrix] ConstantColor [_Color] combine texture * constant }
  SetTexture [_MainTex] { combine texture * previous double, texture alpha * primary alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLMRGBM" "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord1", TexCoord1
   Bind "texcoord", TexCoord2
  }
  AlphaToMask On
  AlphaTest Greater [_Cutoff]
  ColorMask RGB
  Offset -1, -1
  SetTexture [unity_Lightmap] { Matrix [unity_LightmapMatrix] combine texture * texture alpha double }
  SetTexture [unity_Lightmap] { ConstantColor [_Color] combine previous * constant }
  SetTexture [_MainTex] { combine texture * previous quad, texture alpha * primary alpha }
 }
 Pass {
  Name "CASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  ZTest Less
  Cull Off
  Fog { Mode Off }
  Offset 1, 1
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD1;
uniform highp vec4 _MainTex_ST;

uniform highp vec4 unity_LightShadowBias;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (gl_ModelViewProjectionMatrix * _glesVertex);
  tmpvar_1.xyw = tmpvar_2.xyw;
  tmpvar_1.z = (tmpvar_2.z + unity_LightShadowBias.x);
  tmpvar_1.z = mix (tmpvar_1.z, max (tmpvar_1.z, (tmpvar_2.w * -1.0)), unity_LightShadowBias.y);
  gl_Position = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD1;
uniform lowp vec4 _Color;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
void main ()
{
  lowp float x_1;
  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD1).w * _Color.w) - _Cutoff);
  if ((x_1 < 0.0)) {
    discard;
  };
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}



#endif"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 _Object2World;

uniform highp vec4 _LightPositionRange;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_Object2World * _glesVertex).xyz - _LightPositionRange.xyz);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform highp vec4 _LightPositionRange;
void main ()
{
  lowp float x_1;
  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD1).w * _Color.w) - _Cutoff);
  if ((x_1 < 0.0)) {
    discard;
  };
  highp vec4 tmpvar_2;
  tmpvar_2 = fract((vec4(1.0, 255.0, 65025.0, 1.60581e+08) * (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)));
  gl_FragData[0] = (tmpvar_2 - (tmpvar_2.yzww * 0.00392157));
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"!!GLES"
}
}
 }
 Pass {
  Name "SHADOWCOLLECTOR"
  Tags { "LIGHTMODE"="SHADOWCOLLECTOR" "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  ZTest Less
  Fog { Mode Off }
  Offset -1, -1
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 _Object2World;


uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (_Object2World * _glesVertex);
  tmpvar_1.xyz = tmpvar_2.xyz;
  tmpvar_1.w = -((gl_ModelViewMatrix * _glesVertex).z);
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_2).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_2).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_2).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_2).xyz;
  xlv_TEXCOORD4 = tmpvar_1;
  xlv_TEXCOORD5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2D _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
uniform highp vec4 _LightSplitsFar;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _ProjectionParams;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 res_2;
  highp vec4 zFar_3;
  highp vec4 zNear_4;
  lowp float x_5;
  x_5 = ((texture2D (_MainTex, xlv_TEXCOORD5).w * _Color.w) - _Cutoff);
  if ((x_5 < 0.0)) {
    discard;
  };
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_7;
  tmpvar_7 = vec4(tmpvar_6);
  zNear_4 = tmpvar_7;
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zFar_3 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (zNear_4 * zFar_3);
  highp float tmpvar_11;
  tmpvar_11 = clamp (((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = ((((xlv_TEXCOORD0 * tmpvar_10.x) + (xlv_TEXCOORD1 * tmpvar_10.y)) + (xlv_TEXCOORD2 * tmpvar_10.z)) + (xlv_TEXCOORD3 * tmpvar_10.w));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_12.xy);
  highp float tmpvar_14;
  if ((tmpvar_13.x < tmpvar_12.z)) {
    tmpvar_14 = _LightShadowData.x;
  } else {
    tmpvar_14 = 1.0;
  };
  res_2.x = clamp ((tmpvar_14 + tmpvar_11), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_15;
  highp vec2 tmpvar_16;
  tmpvar_16 = fract((vec2(1.0, 255.0) * (1.0 - (xlv_TEXCOORD4.w * _ProjectionParams.w))));
  enc_15.y = tmpvar_16.y;
  enc_15.x = (tmpvar_16.x - (tmpvar_16.y * 0.00392157));
  res_2.zw = enc_15;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;
varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 _Object2World;


uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (_Object2World * _glesVertex);
  tmpvar_1.xyz = tmpvar_2.xyz;
  tmpvar_1.w = -((gl_ModelViewMatrix * _glesVertex).z);
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_2).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_2).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_2).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_2).xyz;
  xlv_TEXCOORD4 = tmpvar_1;
  xlv_TEXCOORD5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
uniform highp vec4 _LightSplitsFar;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _ProjectionParams;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  lowp float x_6;
  x_6 = ((texture2D (_MainTex, xlv_TEXCOORD5).w * _Color.w) - _Cutoff);
  if ((x_6 < 0.0)) {
    discard;
  };
  bvec4 tmpvar_7;
  tmpvar_7 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_8;
  tmpvar_8 = vec4(tmpvar_7);
  zNear_5 = tmpvar_8;
  bvec4 tmpvar_9;
  tmpvar_9 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_10;
  tmpvar_10 = vec4(tmpvar_9);
  zFar_4 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (zNear_5 * zFar_4);
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = ((((xlv_TEXCOORD0 * tmpvar_11.x) + (xlv_TEXCOORD1 * tmpvar_11.y)) + (xlv_TEXCOORD2 * tmpvar_11.z)) + (xlv_TEXCOORD3 * tmpvar_11.w));
  lowp float tmpvar_13;
  tmpvar_13 = shadow2DEXT (_ShadowMapTexture, tmpvar_12.xyz);
  shadow_3 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (_LightShadowData.x + (shadow_3 * (1.0 - _LightShadowData.x)));
  shadow_3 = tmpvar_14;
  res_2.x = clamp ((shadow_3 + clamp (((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_15;
  highp vec2 tmpvar_16;
  tmpvar_16 = fract((vec2(1.0, 255.0) * (1.0 - (xlv_TEXCOORD4.w * _ProjectionParams.w))));
  enc_15.y = tmpvar_16.y;
  enc_15.x = (tmpvar_16.x - (tmpvar_16.y * 0.00392157));
  res_2.zw = enc_15;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 _Object2World;


uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (_Object2World * _glesVertex);
  tmpvar_1.xyz = tmpvar_2.xyz;
  tmpvar_1.w = -((gl_ModelViewMatrix * _glesVertex).z);
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_2).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_2).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_2).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_2).xyz;
  xlv_TEXCOORD4 = tmpvar_1;
  xlv_TEXCOORD5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2D _ShadowMapTexture;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp vec4 _LightShadowData;
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 _ProjectionParams;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 res_2;
  highp vec4 cascadeWeights_3;
  lowp float x_4;
  x_4 = ((texture2D (_MainTex, xlv_TEXCOORD5).w * _Color.w) - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  highp vec3 tmpvar_5;
  tmpvar_5 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_9;
  tmpvar_9.x = dot (tmpvar_5, tmpvar_5);
  tmpvar_9.y = dot (tmpvar_6, tmpvar_6);
  tmpvar_9.z = dot (tmpvar_7, tmpvar_7);
  tmpvar_9.w = dot (tmpvar_8, tmpvar_8);
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (tmpvar_9, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  cascadeWeights_3 = tmpvar_11;
  cascadeWeights_3.yzw = clamp ((cascadeWeights_3.yzw - cascadeWeights_3.xyz), 0.0, 1.0);
  highp vec3 p_12;
  p_12 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_13;
  tmpvar_13 = clamp (((sqrt(dot (p_12, p_12)) * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = ((((xlv_TEXCOORD0 * cascadeWeights_3.x) + (xlv_TEXCOORD1 * cascadeWeights_3.y)) + (xlv_TEXCOORD2 * cascadeWeights_3.z)) + (xlv_TEXCOORD3 * cascadeWeights_3.w));
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_14.xy);
  highp float tmpvar_16;
  if ((tmpvar_15.x < tmpvar_14.z)) {
    tmpvar_16 = _LightShadowData.x;
  } else {
    tmpvar_16 = 1.0;
  };
  res_2.x = clamp ((tmpvar_16 + tmpvar_13), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_17;
  highp vec2 tmpvar_18;
  tmpvar_18 = fract((vec2(1.0, 255.0) * (1.0 - (xlv_TEXCOORD4.w * _ProjectionParams.w))));
  enc_17.y = tmpvar_18.y;
  enc_17.x = (tmpvar_18.x - (tmpvar_18.y * 0.00392157));
  res_2.zw = enc_17;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;
varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 _Object2World;


uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (_Object2World * _glesVertex);
  tmpvar_1.xyz = tmpvar_2.xyz;
  tmpvar_1.w = -((gl_ModelViewMatrix * _glesVertex).z);
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_2).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_2).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_2).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_2).xyz;
  xlv_TEXCOORD4 = tmpvar_1;
  xlv_TEXCOORD5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec2 xlv_TEXCOORD5;
varying highp vec4 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2DShadow _ShadowMapTexture;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp vec4 _LightShadowData;
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 _ProjectionParams;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  lowp float x_5;
  x_5 = ((texture2D (_MainTex, xlv_TEXCOORD5).w * _Color.w) - _Cutoff);
  if ((x_5 < 0.0)) {
    discard;
  };
  highp vec3 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_10;
  tmpvar_10.x = dot (tmpvar_6, tmpvar_6);
  tmpvar_10.y = dot (tmpvar_7, tmpvar_7);
  tmpvar_10.z = dot (tmpvar_8, tmpvar_8);
  tmpvar_10.w = dot (tmpvar_9, tmpvar_9);
  bvec4 tmpvar_11;
  tmpvar_11 = lessThan (tmpvar_10, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_12;
  tmpvar_12 = vec4(tmpvar_11);
  cascadeWeights_4 = tmpvar_12;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 p_13;
  p_13 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = ((((xlv_TEXCOORD0 * cascadeWeights_4.x) + (xlv_TEXCOORD1 * cascadeWeights_4.y)) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  lowp float tmpvar_15;
  tmpvar_15 = shadow2DEXT (_ShadowMapTexture, tmpvar_14.xyz);
  shadow_3 = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = (_LightShadowData.x + (shadow_3 * (1.0 - _LightShadowData.x)));
  shadow_3 = tmpvar_16;
  res_2.x = clamp ((shadow_3 + clamp (((sqrt(dot (p_13, p_13)) * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_17;
  highp vec2 tmpvar_18;
  tmpvar_18 = fract((vec2(1.0, 255.0) * (1.0 - (xlv_TEXCOORD4.w * _ProjectionParams.w))));
  enc_17.y = tmpvar_18.y;
  enc_17.x = (tmpvar_18.x - (tmpvar_18.y * 0.00392157));
  res_2.zw = enc_17;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"!!GLES"
}
}
 }
}
SubShader { 
 Tags { "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
 Pass {
  Tags { "LIGHTMODE"="Always" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  Lighting On
  AlphaToMask On
  Material {
   Ambient [_Color]
   Diffuse [_Color]
  }
  AlphaTest Greater [_Cutoff]
  ColorMask RGB
  Offset -1, -1
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
}
}
´¢Shader "Decal/Mobile/Cutout Diffuse" {
Properties {
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader { 
 Tags { "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="True" "RenderType"="TransparentCutout" }
  ColorMask RGB
  Offset -1, -1
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _Object2World;

uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _WorldSpaceLightPos0;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp float x_4;
  x_4 = (tmpvar_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  lowp vec4 c_5;
  c_5.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * (max (0.0, dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)) * 2.0));
  c_5.w = tmpvar_3;
  c_1.xyz = (c_5.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;

attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float x_3;
  x_3 = (tmpvar_2.w - _Cutoff);
  if ((x_3 < 0.0)) {
    discard;
  };
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;

attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float x_3;
  x_3 = (tmpvar_2.w - _Cutoff);
  if ((x_3 < 0.0)) {
    discard;
  };
  mediump vec3 lm_4;
  lowp vec3 tmpvar_5;
  tmpvar_5 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz);
  lm_4 = tmpvar_5;
  mediump vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2.xyz * lm_4);
  c_1.xyz = tmpvar_6;
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2D _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _WorldSpaceLightPos0;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp float x_4;
  x_4 = (tmpvar_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  lowp float tmpvar_5;
  mediump float lightShadowDataX_6;
  highp float dist_7;
  lowp float tmpvar_8;
  tmpvar_8 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD3).x;
  dist_7 = tmpvar_8;
  highp float tmpvar_9;
  tmpvar_9 = _LightShadowData.x;
  lightShadowDataX_6 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = max (float((dist_7 > (xlv_TEXCOORD3.z / xlv_TEXCOORD3.w))), lightShadowDataX_6);
  tmpvar_5 = tmpvar_10;
  lowp vec4 c_11;
  c_11.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * ((max (0.0, dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)) * tmpvar_5) * 2.0));
  c_11.w = tmpvar_3;
  c_1.xyz = (c_11.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float x_3;
  x_3 = (tmpvar_2.w - _Cutoff);
  if ((x_3 < 0.0)) {
    discard;
  };
  lowp float tmpvar_4;
  mediump float lightShadowDataX_5;
  highp float dist_6;
  lowp float tmpvar_7;
  tmpvar_7 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD2).x;
  dist_6 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = _LightShadowData.x;
  lightShadowDataX_5 = tmpvar_8;
  highp float tmpvar_9;
  tmpvar_9 = max (float((dist_6 > (xlv_TEXCOORD2.z / xlv_TEXCOORD2.w))), lightShadowDataX_5);
  tmpvar_4 = tmpvar_9;
  c_1.xyz = (tmpvar_2.xyz * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz), vec3((tmpvar_4 * 2.0))));
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float x_3;
  x_3 = (tmpvar_2.w - _Cutoff);
  if ((x_3 < 0.0)) {
    discard;
  };
  lowp float tmpvar_4;
  mediump float lightShadowDataX_5;
  highp float dist_6;
  lowp float tmpvar_7;
  tmpvar_7 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD2).x;
  dist_6 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = _LightShadowData.x;
  lightShadowDataX_5 = tmpvar_8;
  highp float tmpvar_9;
  tmpvar_9 = max (float((dist_6 > (xlv_TEXCOORD2.z / xlv_TEXCOORD2.w))), lightShadowDataX_5);
  tmpvar_4 = tmpvar_9;
  mediump vec3 lm_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz);
  lm_10 = tmpvar_11;
  lowp vec3 tmpvar_12;
  tmpvar_12 = vec3((tmpvar_4 * 2.0));
  mediump vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_2.xyz * min (lm_10, tmpvar_12));
  c_1.xyz = tmpvar_13;
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _Object2World;

uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[4];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  highp vec3 tmpvar_22;
  tmpvar_22 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_4LightPosX0 - tmpvar_22.x);
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_4LightPosY0 - tmpvar_22.y);
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_4LightPosZ0 - tmpvar_22.z);
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_23 * tmpvar_23) + (tmpvar_24 * tmpvar_24)) + (tmpvar_25 * tmpvar_25));
  highp vec4 tmpvar_27;
  tmpvar_27 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_23 * tmpvar_5.x) + (tmpvar_24 * tmpvar_5.y)) + (tmpvar_25 * tmpvar_5.z)) * inversesqrt(tmpvar_26))) * (1.0/((1.0 + (tmpvar_26 * unity_4LightAtten0)))));
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_3 + ((((unity_LightColor[0].xyz * tmpvar_27.x) + (unity_LightColor[1].xyz * tmpvar_27.y)) + (unity_LightColor[2].xyz * tmpvar_27.z)) + (unity_LightColor[3].xyz * tmpvar_27.w)));
  tmpvar_3 = tmpvar_28;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _WorldSpaceLightPos0;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp float x_4;
  x_4 = (tmpvar_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  lowp vec4 c_5;
  c_5.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * (max (0.0, dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)) * 2.0));
  c_5.w = tmpvar_3;
  c_1.xyz = (c_5.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[4];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  highp vec3 tmpvar_22;
  tmpvar_22 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_4LightPosX0 - tmpvar_22.x);
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_4LightPosY0 - tmpvar_22.y);
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_4LightPosZ0 - tmpvar_22.z);
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_23 * tmpvar_23) + (tmpvar_24 * tmpvar_24)) + (tmpvar_25 * tmpvar_25));
  highp vec4 tmpvar_27;
  tmpvar_27 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_23 * tmpvar_5.x) + (tmpvar_24 * tmpvar_5.y)) + (tmpvar_25 * tmpvar_5.z)) * inversesqrt(tmpvar_26))) * (1.0/((1.0 + (tmpvar_26 * unity_4LightAtten0)))));
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_3 + ((((unity_LightColor[0].xyz * tmpvar_27.x) + (unity_LightColor[1].xyz * tmpvar_27.y)) + (unity_LightColor[2].xyz * tmpvar_27.z)) + (unity_LightColor[3].xyz * tmpvar_27.w)));
  tmpvar_3 = tmpvar_28;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2D _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _WorldSpaceLightPos0;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp float x_4;
  x_4 = (tmpvar_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  lowp float tmpvar_5;
  mediump float lightShadowDataX_6;
  highp float dist_7;
  lowp float tmpvar_8;
  tmpvar_8 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD3).x;
  dist_7 = tmpvar_8;
  highp float tmpvar_9;
  tmpvar_9 = _LightShadowData.x;
  lightShadowDataX_6 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = max (float((dist_7 > (xlv_TEXCOORD3.z / xlv_TEXCOORD3.w))), lightShadowDataX_6);
  tmpvar_5 = tmpvar_10;
  lowp vec4 c_11;
  c_11.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * ((max (0.0, dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)) * tmpvar_5) * 2.0));
  c_11.w = tmpvar_3;
  c_1.xyz = (c_11.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2DShadow _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _WorldSpaceLightPos0;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp float x_4;
  x_4 = (tmpvar_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  lowp float shadow_5;
  lowp float tmpvar_6;
  tmpvar_6 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  highp float tmpvar_7;
  tmpvar_7 = (_LightShadowData.x + (tmpvar_6 * (1.0 - _LightShadowData.x)));
  shadow_5 = tmpvar_7;
  lowp vec4 c_8;
  c_8.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * ((max (0.0, dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)) * shadow_5) * 2.0));
  c_8.w = tmpvar_3;
  c_1.xyz = (c_8.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float x_3;
  x_3 = (tmpvar_2.w - _Cutoff);
  if ((x_3 < 0.0)) {
    discard;
  };
  lowp float shadow_4;
  lowp float tmpvar_5;
  tmpvar_5 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD2.xyz);
  highp float tmpvar_6;
  tmpvar_6 = (_LightShadowData.x + (tmpvar_5 * (1.0 - _LightShadowData.x)));
  shadow_4 = tmpvar_6;
  c_1.xyz = (tmpvar_2.xyz * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz), vec3((shadow_4 * 2.0))));
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float x_3;
  x_3 = (tmpvar_2.w - _Cutoff);
  if ((x_3 < 0.0)) {
    discard;
  };
  lowp float shadow_4;
  lowp float tmpvar_5;
  tmpvar_5 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD2.xyz);
  highp float tmpvar_6;
  tmpvar_6 = (_LightShadowData.x + (tmpvar_5 * (1.0 - _LightShadowData.x)));
  shadow_4 = tmpvar_6;
  mediump vec3 lm_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz);
  lm_7 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = vec3((shadow_4 * 2.0));
  mediump vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2.xyz * min (lm_7, tmpvar_9));
  c_1.xyz = tmpvar_10;
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _Object2World;

uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[4];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
attribute vec4 _glesMultiTexCoord0;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  highp vec3 tmpvar_22;
  tmpvar_22 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_4LightPosX0 - tmpvar_22.x);
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_4LightPosY0 - tmpvar_22.y);
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_4LightPosZ0 - tmpvar_22.z);
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_23 * tmpvar_23) + (tmpvar_24 * tmpvar_24)) + (tmpvar_25 * tmpvar_25));
  highp vec4 tmpvar_27;
  tmpvar_27 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_23 * tmpvar_5.x) + (tmpvar_24 * tmpvar_5.y)) + (tmpvar_25 * tmpvar_5.z)) * inversesqrt(tmpvar_26))) * (1.0/((1.0 + (tmpvar_26 * unity_4LightAtten0)))));
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_3 + ((((unity_LightColor[0].xyz * tmpvar_27.x) + (unity_LightColor[1].xyz * tmpvar_27.y)) + (unity_LightColor[2].xyz * tmpvar_27.z)) + (unity_LightColor[3].xyz * tmpvar_27.w)));
  tmpvar_3 = tmpvar_28;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
uniform lowp float _Cutoff;
uniform sampler2D _MainTex;
uniform sampler2DShadow _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _WorldSpaceLightPos0;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp float x_4;
  x_4 = (tmpvar_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  lowp float shadow_5;
  lowp float tmpvar_6;
  tmpvar_6 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  highp float tmpvar_7;
  tmpvar_7 = (_LightShadowData.x + (tmpvar_6 * (1.0 - _LightShadowData.x)));
  shadow_5 = tmpvar_7;
  lowp vec4 c_8;
  c_8.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * ((max (0.0, dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)) * shadow_5) * 2.0));
  c_8.w = tmpvar_3;
  c_1.xyz = (c_8.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES"
}
}
 }
}
Fallback "Decal/Cutout VertexLit"
}
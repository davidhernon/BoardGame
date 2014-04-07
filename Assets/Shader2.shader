       Shader "Custom/Shader2" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
     
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 150
     
    CGPROGRAM
    #pragma surface surf Lambert vertex:vert
     
    sampler2D _MainTex;
    fixed4 _Color;
     
    struct Input {
        float2 uv_MainTex;
        float3 vertColor;
    };
     
    void vert (inout appdata_full v, out Input o) {
        UNITY_INITIALIZE_OUTPUT(Input, o);
        o.vertColor = v.color;
    }
     
    void surf (Input IN, inout SurfaceOutput o) {
        fixed4 c = _Color;
        o.Albedo = IN.vertColor;
        o.Alpha = c.a;
    }
    ENDCG
    }
     
    Fallback "Diffuse"
    }
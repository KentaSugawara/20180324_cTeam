Shader "Custom/HoleViewShader"
{
	Properties{
		_BackColor("background color", Color) = (0, 0, 0, 0) // 背景色
		_HoleX("Hole Position X", float) = 0 // 穴の位置X
		_HoleY("Hole Position Y", float) = 0 // 穴の位置Y
		_Width("hole width", float) = 0 // 穴の横幅
		_Height("hole height", float) = 0 // 穴の縦幅
		_ScreenW("Screen Width" , float) = 960.0 // 画面の幅
		_ScreenH("Screen Height" , float) = 640.0 // 画面の高さ
	}

		SubShader{
		Tags{ "Queue" = "Transparent" } // alphaに対応するのに必要

		Blend SrcAlpha OneMinusSrcAlpha // alphaに対応するために必要
		Pass{
		CGPROGRAM

#include "UnityCG.cginc"
#pragma vertex vert_img
#pragma fragment frag

		// Propertiesの値をShaderに渡す
		float _HoleX;
	float _HoleY;
	fixed4 _BackColor;
	float _Width;
	float _Height;
	float _ScreenW;
	float _ScreenH;

	fixed4 frag(v2f_img i) : COLOR{

		i.uv.x *= _ScreenW;
		i.uv.y *= _ScreenH;
	
	if (_HoleX <= i.uv.x && i.uv.x <= _HoleX + _Width) {
		if (_HoleY <= i.uv.y && i.uv.y <= _HoleY + _Height) {
			discard; // 指定位置より一定距離以内だったら処理を飛ばすだけ
		}
	}
	return _BackColor;
	}
		ENDCG
	}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Texture2D : MonoBehaviour {

    [SerializeField]
    private MeshRenderer _Image;

    Texture2D _tex2D;

    public Texture2D ToTexture2D(Texture self)
    {
        var sw = self.width;
        var sh = self.height;
        var format = TextureFormat.RGBA32;
        var result = new Texture2D(sw, sh, format, false);
        var currentRT = RenderTexture.active;
        var rt = new RenderTexture(sw, sh, 32);
        Graphics.Blit(self, rt);
        RenderTexture.active = rt;
        var source = new Rect(0, 0, rt.width, rt.height);
        result.ReadPixels(source, 0, 0);
        result.Apply();
        RenderTexture.active = currentRT;
        return result;
    }

    private Texture2D Resize(Texture2D texture, int width, int height)
    {
        Color[] Pixels = texture.GetPixels();
        Texture2D newTexture = new Texture2D(width, height, texture.format, false);
        Color[] newPixels = new Color[width * height];

        float wRatio = (float)texture.width / width;
        float hRatio = (float)texture.height / height;
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int index = (int)(i * hRatio) * width + (int)(j * wRatio);
                int index2 = i * width + j;
                newPixels[i * width + j] = Pixels[(int)(i * hRatio) * width + (int)(j * wRatio)];
            }
        }

        newTexture.SetPixels(newPixels);
        newTexture.filterMode = FilterMode.Point;
        newTexture.Apply();
        //texture = newTexture;
        return newTexture;
    }

    // Use this for initialization
    void Start () {
        _tex2D = ToTexture2D(_Image.material.mainTexture);
    }
	
    public void Saving()
    {
        Debug.Log(_tex2D.width + " " + _tex2D.height);
        //_tex2D.Resize((int)(_tex2D.width * 0.9f), (int)(_tex2D.height * 0.9f), TextureFormat.RGBA32, false);
        //_tex2D = Resize(_tex2D, (int)(_tex2D.width * 1.0f), (int)(_tex2D.height * 0.9f));
        TextureScale.Bilinear(_tex2D, (int)(_tex2D.width * 0.1f), (int)(_tex2D.height * 0.1f));

        //_tex2D.Apply();

        _Image.material.mainTexture = _tex2D;
    }

	// Update is called once per frame
	void Update () {
		
	}
}

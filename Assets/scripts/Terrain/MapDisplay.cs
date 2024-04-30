using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    
    public GameObject meshObject;
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    public void DrawTexture(Texture2D texture){
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width,1,texture.height);
    }

    public void DrawMesh(MeshData meshData){
        MeshCollider mc = meshObject.GetComponent<MeshCollider>();
        if (mc != null)
        {
            Destroy(mc);
        }
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshCollider = meshObject.AddComponent<MeshCollider>();
    }

}

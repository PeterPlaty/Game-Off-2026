// Mesh Generator created following Brackeys tutorial series
using UnityEngine;

[RequireComponent(typeof(MeshFilter))] //always a mesh filter on the object
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;
    public int unitSize = 1;
    float noiseScale = .15f;
    float maxHeight  = 30f;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    
        // Generate mesh shape
        CreateShape();
        UpdateMesh();

        // Add collisions at runtime
        MeshCollider col = gameObject.AddComponent<MeshCollider>();
        col.sharedMesh = mesh;
    }

    void CreateShape()
    {
        // Generate vertices
        vertices = new Vector3[(xSize+1) * (zSize+1)];

        for(int i=0, z=0; z<=zSize; z++)
        {
            for(int x=0; x<=xSize; x++)
            {
                float y=Mathf.PerlinNoise(x*noiseScale, z*noiseScale) * maxHeight;

                vertices[i] = new Vector3(x*unitSize, y, z*unitSize);
                i++;
            }
        }

        // Generate triangles
        triangles = new int[xSize * zSize * 6];
        
        int vert=0; // vertex currently considered
        int tris=0; // triangle currently considered
        
        for(int z=0; z<zSize; z++)
        {
            for(int x=0; x<xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
            
                vert++;
                tris+=6;
            }

            // Avoid creating a new triangle when wrapping around
            vert++;
        }
    }

    void UpdateMesh()
    {
        // Clear mesh of previous data
        mesh.Clear();

        // Give new data
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}

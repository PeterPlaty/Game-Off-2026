// Mesh Generator created following Brackeys tutorial series
using UnityEngine;

[RequireComponent(typeof(MeshFilter))] //always a mesh filter on the object
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;
    public int unitSize = 1;    // how big a triangle is
    public float noise01Scale = .2f;
    public float noise01Amp  = 2f;
    public float noise02Scale = .4f;
    public float noise02Amp = 4f;
    public float noise03Scale = .8f;
    public float noise03Amp = 8f;
    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    

        // Add collisions at runtime
        //MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        //collider.sharedMesh = mesh;
    }

    private void Update()
    {
        // Generate mesh shape
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        // Generate vertices
        vertices = new Vector3[(xSize+1) * (zSize+1)];

        for(int i=0, z=0; z<=zSize; z++)
        {
            for(int x=0; x<=xSize; x++)
            {
                float y = GetNoiseSample(x, z);
                vertices[i] = new Vector3(x * unitSize, y, z * unitSize);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;

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

        // Define UVs
        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }

    }

    void UpdateMesh()
    {
        // Clear mesh of previous data
        mesh.Clear();

        // Give new data
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    private float GetNoiseSample(int x, int z)
    {
        float sample = 0f;

        sample += Mathf.PerlinNoise(x * noise01Scale / xSize, z * noise01Scale / zSize) * noise01Amp;
        sample += Mathf.PerlinNoise(x * noise02Scale / xSize, z * noise02Scale / zSize) * noise02Amp;
        sample += Mathf.PerlinNoise(x * noise03Scale / xSize, z * noise03Scale / zSize) * noise03Amp;

        return sample;
    }
}

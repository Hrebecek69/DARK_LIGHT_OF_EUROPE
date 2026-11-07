
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteAlphaMeshCollider))]
public class SpriteAlphaMeshColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpriteAlphaMeshCollider generator =
            (SpriteAlphaMeshCollider)target;

        if (GUILayout.Button("Generate 3D Mesh Collider"))
        {
            Generate(generator);
        }
    }

    void Generate(SpriteAlphaMeshCollider generator)
    {
        SpriteRenderer sr = generator.GetComponent<SpriteRenderer>();
        MeshCollider collider = generator.GetComponent<MeshCollider>();

        if (sr == null || sr.sprite == null || collider == null)
        {
            Debug.LogError("Chybí SpriteRenderer, Sprite nebo MeshCollider!");
            return;
        }

        Sprite sprite = sr.sprite;
        Texture2D texture = sprite.texture;

        if (!texture.isReadable)
        {
            Debug.LogError("Zapni Read/Write u PNG!");
            return;
        }

        if (sprite.packed)
        {
            Debug.LogError("Sprite je v Atlasu. Nejdøív ho rozbal.");
            return;
        }

        Rect rect = sprite.textureRect;

        int startX = Mathf.RoundToInt(rect.x);
        int startY = Mathf.RoundToInt(rect.y);
        int width = Mathf.RoundToInt(rect.width);
        int height = Mathf.RoundToInt(rect.height);

        Color[] pixels = texture.GetPixels(
            startX, startY, width, height
        );

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float ppu = sprite.pixelsPerUnit;

        for (int y = 0; y < height; y++)
        {
            int x = 0;

            while (x < width)
            {
                while (x < width &&
                    pixels[y * width + x].a <= generator.alphaThreshold)
                    x++;

                if (x >= width)
                    break;

                int left = x;

                while (x < width &&
                    pixels[y * width + x].a > generator.alphaThreshold)
                    x++;

                int right = x;

                float x0 = (left + rect.x - sprite.rect.x - sprite.pivot.x) / ppu;
                float x1 = (right + rect.x - sprite.rect.x - sprite.pivot.x) / ppu;

                float y0 = (y + rect.y - sprite.rect.y - sprite.pivot.y) / ppu;
                float y1 = (y + 1 + rect.y - sprite.rect.y - sprite.pivot.y) / ppu;

                if (sr.flipX)
                {
                    x0 = -x0;
                    x1 = -x1;
                }

                if (sr.flipY)
                {
                    y0 = -y0;
                    y1 = -y1;
                }

                int index = vertices.Count;

                vertices.Add(new Vector3(x0, y0, 0));
                vertices.Add(new Vector3(x1, y0, 0));
                vertices.Add(new Vector3(x1, y1, 0));
                vertices.Add(new Vector3(x0, y1, 0));

                bool reverse = sr.flipX == sr.flipY;

                if (!reverse)
                {
                    triangles.Add(index);
                    triangles.Add(index + 1);
                    triangles.Add(index + 2);
                    triangles.Add(index);
                    triangles.Add(index + 2);
                    triangles.Add(index + 3);
                }
                else
                {
                    triangles.Add(index);
                    triangles.Add(index + 2);
                    triangles.Add(index + 1);
                    triangles.Add(index);
                    triangles.Add(index + 3);
                    triangles.Add(index + 2);
                }
            }
        }

        if (vertices.Count == 0)
        {
            Debug.LogError("Nebyly nalezeny žádné neprùhledné pixely!");
            return;
        }

        Mesh mesh = new Mesh();
        mesh.name = generator.name + "_Collider";

        if (vertices.Count > 65535)
            mesh.indexFormat =
                UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        string folder = "Assets/GeneratedRegionMeshes";

        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets", "GeneratedRegionMeshes");

        string path = AssetDatabase.GenerateUniqueAssetPath(
            folder + "/" + generator.name + "_Collider.asset"
        );

        AssetDatabase.CreateAsset(mesh, path);

        Undo.RecordObject(collider, "Generate Region Collider");

        collider.sharedMesh = null;
        collider.convex = false;
        collider.sharedMesh = mesh;

        EditorUtility.SetDirty(collider);
        AssetDatabase.SaveAssets();

        Debug.Log("Collider vytvoøen: " + generator.name);
    }
}

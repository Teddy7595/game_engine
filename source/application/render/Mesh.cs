using System;
using System.Numerics;
using System.Linq;

namespace Application.Render
{
    public struct MeshAsset
    {
        public float[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public float[] Normals { get; private set; }
        public float[] UVs { get; private set; }
        public int VertexCount => Vertices.Length / 3;
        public int IndexCount => Indices.Length;
        public Guid AssetId { get; private set; }

        public MeshAsset(Guid id, float[] vertices, uint[] indices) : this(id, vertices, indices, [], []) { }

        public MeshAsset(Guid id, float[] vertices, uint[] indices, float[] normals, float[] uvs)
        {
            Vertices = vertices ?? Array.Empty<float>();
            Indices = indices ?? Array.Empty<uint>();
            Normals = normals ?? [];
            UVs = uvs ?? [];
            AssetId = id;
        }
    }
}
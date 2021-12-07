
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

//[ExecuteInEditMode]

public class treDis : MonoBehaviour
{
    const int chanku = 250;
    int wait;
    public int Htexno;
    public bool r16flag;
    public int offsetT = 1;
    public Vector2 offset;
    public Vector2 offsetx8;
    Vector3[] vertices;
    Vector3[] vertices_l2;
    Vector3[] vertices_l3;
    Color32[] VC;
    Color32[] VC_l2;
    Color32[] VC_l3;

    public int cut=8;
    Vector2[] uvs;
    Vector2[] uvs_l2;
    Vector2[] uvs_l2_h;
    Vector2[] uvs_l3_h;
    Vector2[] uvs_l3;
    MeshFilter mf;
    public Mesh meshsere3;
    public Mesh meshsere2;
    public Mesh meshsere;
    public Mesh mesh;
    public Mesh meshLod2;
    public Mesh meshLod3;
 
    Color32[] spcc;
    public MeshCollider meshc;
    int w;
    int h;
    int ow;
    int oh;
    int spw;
    int sph;
    public int iti_x;
    public int iti_y;
    bool kanl2,kanl3;
    bool mesu;
    public bool meshari;
    public bool meshari2;
    public bool meshari3;
    public float dis=1;
    float texselX;
    float texselY;
    float fx,fy;
    float spcut;
    public Texture2D heightTex;
    public Texture2D splatTex;
    public Color[] cc;


    private void Start()
    {
        w = heightTex.width;
        h = heightTex.height;
        spw = splatTex.width;
        sph = splatTex.height;
        mf = GetComponent<MeshFilter>();
        meshc = GetComponent<MeshCollider>();
        texselX = 1.0f / w;
        texselY = 1.0f / h;
        offset = new Vector2(offset.x+(offsetx8.x*8),offset.y+(offsetx8.y*8));
        spcut = cut;
        /////////////////////////////////////////////////////////////////////
        ///読み込み
        cc = heightTex.GetPixels(0);
        spcc = splatTex.GetPixels32(0);

        /////////////////////////////////////////////////////////////////////
        mesh1();
        mesh2();
        mesh3();

    }
    public void mesh1()
    {
        mesh = Instantiate(meshsere);
        vertices = mesh.vertices;
        VC = new Color32[vertices.Length];
        uvs = mesh.uv;
        mesh.uv3 = uvs;
        ExMeshJob();
    }
    public void mesh2()
    {
        meshLod2 = Instantiate(meshsere2);
        vertices_l2 = meshLod2.vertices;
        VC_l2 = new Color32[vertices_l2.Length];
        uvs_l2 = meshsere2.uv2;
        uvs_l2_h = meshsere2.uv2;
        meshLod2.uv3 = meshsere2.uv2;
        ExMeshJobL2();
    }
    public void mesh3()
    {
        meshLod3 = Instantiate(meshsere3);
        vertices_l3 = meshLod3.vertices;
        VC_l3 = new Color32[vertices_l3.Length];
        uvs_l3_h = meshsere3.uv2;
        uvs_l3 = meshsere3.uv2;
        meshLod3.uv3 = meshsere3.uv2;
        ExMeshJobL3();
    }


    [BurstCompile]
    struct MeshVJ : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> verticesA;
      //  [ReadOnly] public NativeArray<Color32> VCA;
        public NativeArray<Vector2> uvsA;
        [WriteOnly]public NativeArray<Vector3> verticesAO;
        [WriteOnly] public NativeArray<Color32> VCAO;
        [WriteOnly]public NativeArray<Vector2> uvsAO;
        [ReadOnly] public NativeArray<Color> cc;
        [ReadOnly] public NativeArray<Color32> spcc;
        Vector2 uvO;
        public float spcut;
        public float dis;
        public Vector2 offset;
        public int w, h,spw,sph;
        public int offsetT;
        public int cut;
        int d0, d1, d2, d3, d4;
        bool xf1, xf2, xf3, xf4;
        bool yf1, yf2, yf3, yf4;
        float cf1, cf2, cf3, cf4;
        int s0;
        float disp;
        public bool r16flag;
        public float uvcut;
        public bool uvcf;
        int w2;
        int h2;
        void IJobParallelFor.Execute(int id)
        {
            w2 = w;
            h2 = h;
            uvO = new Vector2(uvsA[id].x / spcut + (1.0f / spcut) * offset.x, uvsA[id].y / spcut + (1.0f / spcut) * offset.y);
            uvsA[id] = new Vector2(uvsA[id].x / cut + (1.0f / cut) * offset.x, uvsA[id].y / cut + (1.0f / cut) * offset.y);
                float texsel = 1.0f / w;
                float texselH = texsel * 0.5f;
                int x2 = (int)(uvsA[id].x * w);
                int y2 = (int)(uvsA[id].y * h);
                int tx1 = (int)((uvsA[id].x+ texselH) * w);
                int ty1 = (int)((uvsA[id].y+ texselH) * h);
                int tx2 = (int)((uvsA[id].x + texselH) * w);
                int ty2 = (int)((uvsA[id].y - texselH) * h);
                int tx3 = (int)((uvsA[id].x - texselH) * w);
                int ty3 = (int)((uvsA[id].y + texselH) * h);
                int tx4 = (int)((uvsA[id].x - texselH) * w);
                int ty4 = (int)((uvsA[id].y - texselH) * h);
                int spx = (int)(uvO.x * spw);
                int spy = (int)(uvO.y * sph);
                float fx = ((uvsA[id].x-texselH) * w) % 1f;
                float fy = ((uvsA[id].y-texselH) * h) % 1f;


              s0 = spx + spy * spw;
              d0 = x2 + y2 * w2;
              d1 = tx1 + ty1 * w2;
              d2 = tx2 + ty2 * w2;
              d3 = tx3 + ty3 * w2;
              d4 = tx4 + ty4 * w2;

                if (d0 > cc.Length-1) d0 = cc.Length - 1;
                if (d0 < 0) d0 = 0;
                if (s0 > spcc.Length - 1) s0 = spcc.Length - 1;
                if (s0 < 0) s0 = 0;
                if (d1 > cc.Length-1) d1 = d0;
                if (d1 < 0) d1 = d0;
                if (d2 > cc.Length-1) d2 = d0;
                if (d2 < 0) d2 = d0;
                if (d3 > cc.Length-1) d3 = d0;
                if (d3 < 0) d3 = d0;
                if (d4 > cc.Length-1) d4 = d0;
                if (d4 < 0) d4 = d0;

            cf1 = cc[d1].r;
            cf2 = cc[d2].r;
            cf3 = cc[d3].r;
            cf4 = cc[d4].r;
            float dx = (cf4 * (1 - fx)) + (cf2 * fx);
            float dx2 = (cf3 * (1 - fx)) + (cf1 * fx);
            float dispr = (dx * (1 - fy)) + (dx2 * fy);

            disp = dispr*10f;
            
            verticesAO[id] = new Vector3(verticesA[id].x, verticesA[id].y+disp, verticesA[id].z);
            if(uvcf == true)
            {
                uvsAO[id] = new Vector2(uvsA[id].x*uvcut, uvsA[id].y*uvcut);
            }
            else
            {
               
            }
            VCAO[id] = new Color32(spcc[s0].r,spcc[s0].g,spcc[s0].b,0);
            
        }
    }

    void ExMeshJob()
    {
        var verticesA = new NativeArray<Vector3>(vertices,Allocator.Persistent);
        var verticesAO = new NativeArray<Vector3>(vertices.Length, Allocator.Persistent);
        var uvsA = new NativeArray<Vector2>(uvs, Allocator.Persistent);
        var uvsAO = new NativeArray<Vector2>(uvs.Length, Allocator.Persistent);
        var VCAO = new NativeArray<Color32>(VC.Length, Allocator.Persistent);
        var ccA = new NativeArray<Color>(cc, Allocator.Persistent);
        var spccA = new NativeArray<Color32>(spcc, Allocator.Persistent);
        ///////////////////////////////////
        var Tjob = new MeshVJ();

        Tjob.verticesA = verticesA;
        Tjob.uvsA = uvsA;
        Tjob.verticesAO = verticesAO;
        Tjob.uvsAO = uvsAO;
        Tjob.cc = ccA;
        Tjob.spcc = spccA;
        Tjob.VCAO = VCAO;
        Tjob.offset = offset;
        Tjob.dis = dis;
        Tjob.cut = cut;
        Tjob.spcut = spcut;
        Tjob.w = w;
        Tjob.h = h;
        Tjob.spw = spw;
        Tjob.sph = sph;
        Tjob.uvcf = false;
        Tjob.uvcut = 1f;
        Tjob.r16flag=r16flag;
        Tjob.offsetT = offsetT;
        var jobhandle = Tjob.Schedule(verticesA.Length,1000);
        jobhandle.Complete();
        vertices = verticesAO.ToArray();
        uvs = uvsA.ToArray();
        VC = VCAO.ToArray();
        
        verticesA.Dispose();
        uvsA.Dispose();
        ccA.Dispose();
        spccA.Dispose();
        uvsAO.Dispose();
        verticesAO.Dispose();
        VCAO.Dispose();
        mesu = true;
    }
    void ExMeshJobL2()
    {
        var verticesA = new NativeArray<Vector3>(vertices_l2, Allocator.Persistent);
        var verticesAO = new NativeArray<Vector3>(vertices_l2.Length, Allocator.Persistent);
        var uvsA = new NativeArray<Vector2>(uvs_l2, Allocator.Persistent);
        var uvsAO = new NativeArray<Vector2>(uvs_l2.Length, Allocator.Persistent);
        var ccA = new NativeArray<Color>(cc, Allocator.Persistent);
        var VCAO = new NativeArray<Color32>(VC_l2.Length, Allocator.Persistent);
        var spccA = new NativeArray<Color32>(spcc, Allocator.Persistent);
        //////////////////////////////
        var Tjob = new MeshVJ();
        Tjob.verticesA = verticesA;
        Tjob.uvsA = uvsA;
        Tjob.verticesAO = verticesAO;
        Tjob.uvsAO = uvsAO;
        Tjob.spcc = spccA;
        Tjob.cc = ccA;
        Tjob.VCAO = VCAO;
        Tjob.offset = offset;
        Tjob.dis = dis;
        Tjob.cut = cut;
        Tjob.spcut = spcut;
        Tjob.w = w;
        Tjob.h = h;
        Tjob.spw = spw;
        Tjob.sph = sph;
        Tjob.uvcf = false;
        Tjob.uvcut = 1f;
        Tjob.r16flag = r16flag;
        Tjob.offsetT = offsetT;
        var jobhandle = Tjob.Schedule(verticesA.Length, 250);
        jobhandle.Complete();
        vertices_l2 = verticesAO.ToArray();
        uvs_l2 = uvsA.ToArray();
        uvs_l2_h = uvsA.ToArray();
        VC_l2 = VCAO.ToArray();

        verticesA.Dispose();
        uvsA.Dispose();
        ccA.Dispose();
        uvsAO.Dispose();
        verticesAO.Dispose();
        VCAO.Dispose();
        spccA.Dispose();
        kanl2 = true;
    }
    void ExMeshJobL3()
    {
        var verticesA = new NativeArray<Vector3>(vertices_l3, Allocator.Persistent);
        var verticesAO = new NativeArray<Vector3>(vertices_l3.Length, Allocator.Persistent);
        var uvsA = new NativeArray<Vector2>(uvs_l3, Allocator.Persistent);
        var uvsAO = new NativeArray<Vector2>(uvs_l3.Length, Allocator.Persistent);
        var ccA = new NativeArray<Color>(cc, Allocator.Persistent);
        var VCAO = new NativeArray<Color32>(VC_l3.Length, Allocator.Persistent);
        var spccA = new NativeArray<Color32>(spcc, Allocator.Persistent);
        //////////////////////////////
        var Tjob = new MeshVJ();

        Tjob.verticesA = verticesA;
        Tjob.uvsA = uvsA;
        Tjob.verticesAO = verticesAO;
        Tjob.uvsAO = uvsAO;
        Tjob.cc = ccA;
        Tjob.spcc = spccA;
        Tjob.VCAO = VCAO;
        Tjob.offset = offset;
        Tjob.dis = dis;
        Tjob.cut = cut;
        Tjob.spcut = spcut;
        Tjob.w = w;
        Tjob.h = h;
        Tjob.spw = spw;
        Tjob.sph = sph;
        Tjob.r16flag = r16flag;
        Tjob.uvcf = false;
        Tjob.uvcut = 1f;
        Tjob.offsetT = offsetT;
        var jobhandle = Tjob.Schedule(verticesA.Length, 100);
        jobhandle.Complete();
        vertices_l3 = verticesAO.ToArray();
        uvs_l3 = uvsA.ToArray();
        uvs_l3_h = uvsA.ToArray();
        VC_l3 = VCAO.ToArray();

        verticesA.Dispose();
        uvsA.Dispose();
        ccA.Dispose();
        uvsAO.Dispose();
        verticesAO.Dispose();
        VCAO.Dispose();
        spccA.Dispose();
        kanl3 = true;
    }

   




    private void Update()
    {
        
        if (kanl2 ==true)
        {
            meshLod2.vertices = vertices_l2;       
            meshLod2.uv2 = uvs_l2;
            meshLod2.RecalculateBounds();
            meshLod2.RecalculateNormals();
            meshLod2.RecalculateTangents();
            meshLod2.colors32=VC_l2;
            kanl2 = false;
            meshari2 = true;

        }
        if(kanl3 == true) 
        {
            meshLod3.vertices = vertices_l3;
            meshLod3.uv2 = uvs_l3;
            meshLod3.RecalculateBounds();
            meshLod3.RecalculateNormals();
            meshLod3.RecalculateTangents();
            meshLod3.colors32 = VC_l3;
            kanl3 = false;
            meshari3 = true;
        }
        if (mesu == true)
        {
            mesh.vertices = vertices;
            mesh.uv2 = uvs;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.colors32 = VC;
            mesu = false;
            meshari = true;
            mf.mesh = mesh;
            meshc.sharedMesh = mesh;
        }

    }

}


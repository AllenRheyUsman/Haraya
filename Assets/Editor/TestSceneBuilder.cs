using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TestSceneBuilder
{
    private const string CatModelPath = "Assets/Models/Creatures/cat.glb";
    private const string EnvironmentModelPath = "Assets/Models/Environments/environment 1.glb";
    private const string SpeciesDataPath = "Assets/Data/SpeciesData.json";
    private const string ScenePath = "Assets/Scenes/TestScene.unity";

    [MenuItem("Tools/Build Test Scene")]
    public static void Build()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        camGO.AddComponent<Camera>();
        camGO.AddComponent<AudioListener>();

        var envInstance = InstantiateModel(EnvironmentModelPath, "Environment");
        var envBounds = new Bounds();
        bool hasEnvironment = envInstance != null;

        if (hasEnvironment)
        {
            envBounds = GroundAndMeasure(envInstance);
            AddCollidersForRaycast(envInstance);
        }
        else
        {
            // No environment model yet - fall back to a flat placeholder ground.
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(260f, 1f, 260f);
        }

        if (!TryFindDirectionalLight(envInstance))
        {
            var lightGO = new GameObject("Directional Light");
            var light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
            lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        var catInstance = InstantiateModel(CatModelPath, "Cat");
        Bounds combinedBounds = envBounds;

        if (catInstance != null)
        {
            Bounds rawCatBounds = ComputeBounds(catInstance);
            Vector3 placeXZ = hasEnvironment
                ? new Vector3(envBounds.center.x, 0f, envBounds.center.z)
                : Vector3.zero;

            float surfaceY = hasEnvironment
                ? SampleSurfaceHeight(placeXZ.x, placeXZ.z, envBounds)
                : 0f;

            catInstance.transform.position = new Vector3(placeXZ.x, surfaceY - rawCatBounds.min.y, placeXZ.z);
            Bounds catBounds = ComputeBounds(catInstance);
            combinedBounds = hasEnvironment ? Encapsulate(envBounds, catBounds) : catBounds;
        }

        if (hasEnvironment)
        {
            RemoveRaycastColliders(envInstance);
        }

        float radius = Mathf.Max(combinedBounds.extents.magnitude, 0.5f);
        Vector3 lookTarget = combinedBounds.center;
        Vector3 camDir = new Vector3(-0.6f, 0.35f, -1f).normalized;
        camGO.transform.position = lookTarget - camDir * radius * 2.2f;
        camGO.transform.LookAt(lookTarget);

        var gameManagerGO = new GameObject("GameManager");
        gameManagerGO.AddComponent<GameManager>();

        var careManagerGO = new GameObject("CareManager");
        careManagerGO.AddComponent<CareManager>();

        var bootstrapGO = new GameObject("TestBootstrap");
        var bootstrap = bootstrapGO.AddComponent<TestBootstrap>();

        var speciesAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(SpeciesDataPath);
        if (speciesAsset == null)
        {
            Debug.LogError($"TestSceneBuilder: could not load {SpeciesDataPath}");
        }
        else
        {
            var so = new SerializedObject(bootstrap);
            so.FindProperty("speciesData").objectReferenceValue = speciesAsset;
            so.ApplyModifiedProperties();
        }

        EnsureFolder("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, ScenePath);
        AddSceneToBuildSettings(ScenePath);

        Debug.Log($"TestSceneBuilder: built and saved {ScenePath} " +
                  $"(environment: {hasEnvironment}, cat: {catInstance != null}, combined bounds: {combinedBounds.size})");
    }

    private static GameObject InstantiateModel(string assetPath, string instanceName)
    {
        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (asset == null)
        {
            Debug.LogWarning($"TestSceneBuilder: could not load model at {assetPath} - skipping.");
            return null;
        }

        var instance = (GameObject)PrefabUtility.InstantiatePrefab(asset);
        instance.name = instanceName;
        instance.transform.position = Vector3.zero;
        return instance;
    }

    private static Bounds GroundAndMeasure(GameObject instance)
    {
        Bounds bounds = ComputeBounds(instance);
        instance.transform.position = new Vector3(0f, -bounds.min.y, 0f);
        return ComputeBounds(instance);
    }

    private static bool TryFindDirectionalLight(GameObject root)
    {
        if (root == null)
        {
            return false;
        }

        var lights = root.GetComponentsInChildren<Light>();
        foreach (var l in lights)
        {
            if (l.type == LightType.Directional)
            {
                return true;
            }
        }

        return false;
    }

    private static void AddCollidersForRaycast(GameObject root)
    {
        foreach (var meshFilter in root.GetComponentsInChildren<MeshFilter>())
        {
            if (meshFilter.sharedMesh == null || meshFilter.GetComponent<Collider>() != null)
            {
                continue;
            }

            var collider = meshFilter.gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = meshFilter.sharedMesh;
        }
    }

    private static void RemoveRaycastColliders(GameObject root)
    {
        foreach (var collider in root.GetComponentsInChildren<MeshCollider>())
        {
            Object.DestroyImmediate(collider);
        }
    }

    private static float SampleSurfaceHeight(float x, float z, Bounds envBounds)
    {
        Vector3 origin = new Vector3(x, envBounds.max.y + 500f, z);
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, envBounds.size.y + 1000f))
        {
            return hit.point.y;
        }

        return envBounds.min.y;
    }

    private static Bounds ComputeBounds(GameObject root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(root.transform.position, Vector3.one);
        }

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            b.Encapsulate(renderers[i].bounds);
        }
        return b;
    }

    private static Bounds Encapsulate(Bounds a, Bounds b)
    {
        a.Encapsulate(b);
        return a;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        string parent = Path.GetDirectoryName(path)?.Replace("\\", "/");
        string leaf = Path.GetFileName(path);
        AssetDatabase.CreateFolder(parent, leaf);
    }

    private static void AddSceneToBuildSettings(string path)
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (scenes.Exists(s => s.path == path))
        {
            return;
        }

        scenes.Add(new EditorBuildSettingsScene(path, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}

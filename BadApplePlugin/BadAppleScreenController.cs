using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandbox.Arm;
using UnityEngine;
using static BadAppleSkulls.Util.ReflectionUtils;
using static BadAppleSkulls.Util.PathsUtil;
using Snapping = ULTRAKILL.Cheats.Snapping;

namespace BadAppleSkulls
{
    public class BadAppleScreenController : MonoSingleton<BadAppleScreenController>
    {
        public bool screenPresent;
        
        private static int _screenWidth = 60;
        private static int _screenHeight = 45;
        private static float _offset = 3f;

        private List<bool[,]> matrix;
        private GameObject[,] altarMatrix = new GameObject[_screenWidth, _screenHeight];
        private GameObject[,] skullMatrix = new GameObject[_screenWidth, _screenHeight];

        private SpawnableObject skull;
        private Vector3 normal;
        private RaycastHit hit;

        private int count = 0;

        protected override void Awake()
        {
            var database = (SpawnableObjectsDatabase)GetPrivate(MonoSingleton<HUDOptions>.Instance.GetComponentInChildren<SpawnMenu>(true),
                typeof(SpawnMenu), "objects");
            skull = database.objects.First(obj => obj.name == "Blue Skull");

            Texture2D texture;
            matrix = BadAppleFolder.EnumerateFiles()
                .OrderBy(f => f.Name)
                .Select(file =>
                {
                    texture = new Texture2D(2, 2);
                    var bytes = File.ReadAllBytes(file.FullName);
                    texture.LoadImage(bytes);
                    var m = new bool[_screenWidth, _screenHeight];
                    
                    for (var i = 0; i < _screenWidth; i++)
                    for (var j = 0; j < _screenHeight; j++)
                        m[i, j] = texture.GetPixel(i, j).grayscale > 0.2f;
                        
                    
                    return m;
                })
                .ToList();
        }

        public void PlaceScreenAnchors()
        {
            var arm = MonoSingleton<SandboxArm>.Instance;
            normal = arm.hit.normal;
            hit = arm.hit;
            foreach (var pair in new []
                     {
                         (0, 0),
                         (_screenWidth - 1, 0),
                         (0, _screenHeight - 1),
                         (_screenWidth - 1, _screenHeight - 1)
                     })
            {
                altarMatrix[pair.Item1, pair.Item2] = Instantiate(skull.gameObject,
                    CalculatePropPosition(hit) + new Vector3(_offset * pair.Item1, 0, _offset * pair.Item2),
                    CalculatePropRotation(normal, skull.gameObject.transform.localRotation),
                    arm.GetGoreZone().transform);
                skullMatrix[pair.Item1, pair.Item2] = altarMatrix[pair.Item1, pair.Item2].transform.Find("Cube/SkullBlue").gameObject;

                screenPresent = true;
            }
        }

        private void PlaceScreen()
        {
            for (var i = 0; i < _screenWidth; i++)
            for (var j = 0; j < _screenHeight; j++)
            {
                if (i == 0 || i == _screenWidth - 1)
                    if (j == 0 || j == _screenHeight - 1)
                        continue;
                if (j == 0 || j == _screenHeight - 1)
                    if (i == 0 || i == _screenWidth - 1)
                        continue;
                
                altarMatrix[i, j] = Instantiate(skull.gameObject,
                    CalculatePropPosition(hit) + new Vector3(_offset * i, 0, _offset * j),
                    CalculatePropRotation(normal, skull.gameObject.transform.localRotation),
                    GameObject.Find("Level Info").transform);
                skullMatrix[i, j] = altarMatrix[i, j].transform.Find("Cube/SkullBlue").gameObject;

                screenPresent = true;
            }
        }

        public IEnumerator PlayBadApple()
        {
            PlaceScreen();

            yield return null;
            yield return null;

            foreach (var frame in matrix)
            {
                DisplayFrame(frame);
                yield return null;
                yield return ScreenshotEncode();
            }
        }
        
        IEnumerator ScreenshotEncode()
        {
            yield return new WaitForEndOfFrame();
            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            yield return 0;
 
            var bytes = texture.EncodeToPNG();
            
            File.WriteAllBytes(Application.dataPath + "/../badapple-rendered/badapple-" + count + ".png", bytes);
            count++; 
            DestroyObject( texture );
        }

        private void DisplayFrame(bool[,] frame)
        {
            for (var i = 0; i < _screenWidth; i++)
                for (var j = 0; j < _screenHeight; j++)
                    skullMatrix[i,j].SetActive(frame[i,j]);
        }

        private Vector3 CalculatePropPosition(RaycastHit hit)
        {
            return skull.spawnOffset != 0.0 ? hit.point + hit.normal * skull.spawnOffset : hit.point;
        }
        
        private Quaternion CalculatePropRotation(Vector3 normal, Quaternion baseRotation)
        {
            var angle = MonoSingleton<CameraController>.Instance.rotationY;
            if (Snapping.SnappingEnabled)
                angle = Mathf.Round(angle / 90f) * 90f;
            return Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.AngleAxis(angle, Vector3.up) * baseRotation;
        }
    }
}
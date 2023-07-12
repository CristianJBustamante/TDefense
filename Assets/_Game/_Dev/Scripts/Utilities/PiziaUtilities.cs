using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.Pizia.Tools
{
    /// <summary>
    /// Class with a cached Transform Property that you can inhert from
    /// </summary>
    public abstract class CachedReferences : MonoBehaviour
    {
        Transform _MyTransform;
        public Transform MyTransform
        {
            get
            {
                if (_MyTransform == null) _MyTransform = transform;
                return _MyTransform;
            }
        }

        GameObject _MyGameObject;
        public GameObject MyGameObject
        {
            get
            {
                if (_MyGameObject == null) _MyGameObject = gameObject;
                return _MyGameObject;
            }
        }

        RectTransform _MyRectTransform;
        public RectTransform MyRectTransform
        {
            get
            {
                if (_MyRectTransform == null) _MyRectTransform = GetComponent<RectTransform>();
                return _MyRectTransform;
            }
        }
    }

    public static class PiziaUtilities
    {
        class NamesDataWrapper { public string[] Names; }

        static MaterialPropertyBlock _PropertyBlock;
        static MaterialPropertyBlock PropertyBlock
        {
            get
            {
                if (_PropertyBlock == null) _PropertyBlock = new MaterialPropertyBlock();
                return _PropertyBlock;
            }
        }

        static readonly string[] randomNames = new string[84]
        {
            "AnhartEvents",    "Aurance",       "Beautyho",
            "Blottos",         "Brainer",       "Buffer",
            "ConfidentFox",    "Drummiq",       "Essencap", 
            "Grindergo",       "Hannahli",      "Hearter",
            "Ianae",           "Kubassa",       "LatinaMew",
            "Lucam",           "Mcff",          "Meriton",
            "Nicera",          "Niphq",         "Nousen", 
            "Onlinete",        "PhobicXbox",    "Readerer",
            "Realisman",       "Simbaka",       "Sohnet", 
            "Surefr",          "Trustat",       "Twittergi",
            "Baklary",         "BugHelp",       "Burketo", 
            "Cheenp",          "Cheens",        "ClassyPuppy",
            "Clessal",         "FollowChic",    "Hoopel", 
            "HulkLive",        "LinkinPanet",   "Melro",
            "Melyt",           "Metexpe",       "Mobinery", 
            "Oceansh",         "PlotStorm",     "Portecen",
            "ProdigyMaxi",     "Questiss",      "RaeTale", 
            "Saking",          "Sisterun",      "SisTheborg",
            "Smarterer",       "TerrificDream", "Vibrant", 
            "VibrantArsenal",  "Weirdue",       "Wzyrl",
            "Tyrrex",          "IHaveHands",    "Tyrasaurus Rex", 
            "Disguised Frog",  "FrogMilk",      "Gamerfrog",
            "The Gaming Frog", "ScaryGem",      "SlightKnight", 
            "SinfulGem",       "WaryDragon",    "MoonlitSteam",
            "BooshSpy",        "Masterve",      "Micanvil", 
            "Shgiters",        "Cheelysi",      "Followla",
            "Louleaf",         "RozWakeboard",  "Smoothouse", 
            "Cutieti",         "Micanvil",      "Shgiters"
        };

        public static string GenerateRandomName() => randomNames[Random.Range(0, randomNames.Length)];

        public static string GenerateRandomNameJson()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("RandomNames");
            NamesDataWrapper categoryWords = JsonUtility.FromJson<NamesDataWrapper>(jsonFile.text);
            return categoryWords.Names[Random.Range(0, categoryWords.Names.Length)];
        }

        public static Color GetPredominantColorFormSprite(Sprite spriteToAnalyse)
        {
            int atlasWidth = spriteToAnalyse.texture.width;
            int width = Mathf.RoundToInt(spriteToAnalyse.rect.width), height = Mathf.RoundToInt(spriteToAnalyse.rect.height);
            Vector2Int startPoint = new Vector2Int(Mathf.RoundToInt(spriteToAnalyse.rect.center.x - (width / 2)), Mathf.RoundToInt(spriteToAnalyse.rect.center.y - (height / 2)));

            //int start = startPoint.x + (startPoint.y * atlasWidth);
            int size = width * height;
            Vector3 rgb = Vector3.zero;

            Color[] pixels = spriteToAnalyse.texture.GetPixels(startPoint.x, startPoint.y, width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = /*start +*/ x + (y * width);

                    rgb.x += pixels[i].r;
                    rgb.y += pixels[i].g;
                    rgb.z += pixels[i].b;
                }
            }

            return new Color(rgb.x / size, rgb.y / size, rgb.z / size, 1);
        }

        public static Color GetPredominantColor(Texture2D textureToAnalyse)
        {
            Color[] pixels = textureToAnalyse.GetPixels();
            int size = pixels.Length;

            Vector3 rgb = Vector3.zero;
            for (int i = 0; i < size; i++)
            {
                rgb.x += pixels[i].r;
                rgb.y += pixels[i].g;
                rgb.z += pixels[i].b;
            }

            return new Color(rgb.x / size,rgb.y / size, rgb.z / size, 1);
        }

        public static List<int> GenerateRandomNumberArray(int quantity, int maxRange)
        {
            List<int> randomOrder = new List<int>();

            for (int i = 0; i < quantity; i++)
            {
                int random = Random.Range(0, maxRange);

                while (randomOrder.Contains(random))
                    random = Random.Range(0, maxRange);

                randomOrder.Add(random);
            }

            return randomOrder;
        }

        public static List<int> GenerateRandomNumberArray(int quantity, int maxRange, params int[] numbersToIgnore)
        {
            int size = numbersToIgnore.Length;
            List<int> randomOrder = new List<int>();

            for (int i = 0; i < quantity; i++)
            {
                int random = Random.Range(0, maxRange);

                while (randomOrder.Contains(random) || ShouldIgnoreValue(random))
                    random = Random.Range(0, maxRange);

                randomOrder.Add(random);
            }

            return randomOrder;

            bool ShouldIgnoreValue(int value)
            {
                for (int i = 0; i < size; i++)
                {
                    if (value == numbersToIgnore[i])
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Activates a ragdoll and sets its rig to the non ragdoll mesh one
        /// </summary>
        /// <param name="ragdoll">The ragdoll gameObject</param>
        /// <param name="ragdollAnimator">The ragdoll animator</param>
        /// <param name="animator">The main mesh animator to copy the animation from</param>
        /// <param name="mainMesh">The main mesh gameobject</param>
        public static IEnumerator Ragdoll(GameObject ragdoll, Animator ragdollAnimator, Animator animator, GameObject mainMesh)
        {
            ragdoll.SetActive(true);
            ragdollAnimator.enabled = true;

            AnimatorClipInfo currentClipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];

            int currentFrame = (int) (currentClipInfo.weight * (currentClipInfo.clip.length * currentClipInfo.clip.frameRate));
            ragdollAnimator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, currentFrame / currentClipInfo.clip.length);

            mainMesh.SetActive(false);
            yield return null;

            ragdollAnimator.enabled = false;
        }

        public static bool MouseOverUI()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static void Shuffle<T>(ref T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static void Shuffle<T>(ref List<T> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static void ClearMaterialPropertyBlock() => PropertyBlock.Clear();

        /// <summary>
        /// Change the material color using a cached <c>MaterialPropertyBlock</c>
        /// </summary>
        /// <remarks>
        /// If you also try to change the texture of another material with this method, you will also change
        /// the color.<br></br>
        /// Because the <c>MaterialPropertyBlock</c> its not cleared.<br></br>
        /// To avoid this issue, use the <c>ClearMaterialPropertyBlock</c> method after using this one.
        /// </remarks>
        /// <param name="render">The Renderer to apply the PropertyBlock</param>
        /// <param name="newColor">The new color value to apply</param>
        /// <param name="propertyName">(Optional) The name of the property</param>
        public static void ChangeMaterialColor(Renderer render, Color newColor, string propertyName = "_Color")
        {
            PropertyBlock.SetColor(propertyName, newColor);
            render.SetPropertyBlock(PropertyBlock);
        }

        /// <summary>
        /// Change the material texture using a cached <c>MaterialPropertyBlock</c>
        /// </summary>
        /// <remarks>
        /// If you also try to change the color of another material with this method, you will also change
        /// the texture.<br></br>
        /// Because the <c>MaterialPropertyBlock</c> its not cleared.<br></br>
        /// To avoid this issue, use the <c>ClearMaterialPropertyBlock</c> method after using this one.
        /// </remarks>
        /// <param name="render">The Renderer to apply the PropertyBlock</param>
        /// <param name="newTexture">The new texture value to apply</param>
        /// <param name="propertyName">(Optional) The name of the property</param>
        public static void ChangeMaterialTexture(Renderer render, Texture2D newTexture, string propertyName = "_MainTexture")
        {
            PropertyBlock.SetTexture(propertyName, newTexture);
            render.SetPropertyBlock(PropertyBlock);
        }

        /// <summary>
        /// Change the material color and texture using a cached <c>MaterialPropertyBlock</c>
        /// </summary>
        /// <param name="render">The Renderer to apply the PropertyBlock</param>
        /// <param name="newColor">The new color value to apply</param>
        /// <param name="newTexture">The new texture value to apply</param>
        /// <param name="colorPropertyName">(Optional) The name of the color property</param>
        /// <param name="texturePropertyName">(Optional) The name of the texture property</param>
        public static void ChangeMaterialColorAndTexture(Renderer render, Color newColor, Texture2D newTexture, string colorPropertyName = "_Color", string texturePropertyName = "_MainTexture")
        {
            PropertyBlock.SetColor(colorPropertyName, newColor);
            PropertyBlock.SetTexture(texturePropertyName, newTexture);
            render.SetPropertyBlock(PropertyBlock);
        }

        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        /// <param name="x">The number to map </param>
        /// <param name="in_min">The min input value of x </param>
        /// <param name="in_max">The max input value of x </param>
        /// <param name="out_min">The minimum value to output </param>
        /// <param name="out_max">The maximum value to output </param>
        /// <returns>The x parameter mapped to a range between out_min and out_max </returns>
        public static float Map(float x, float in_min, float in_max, float out_min, float out_max) => ((x - in_min) * (out_max - out_min) / (in_max - in_min)) + out_min;

        public static float DistanceFromPointToPlane(Vector2 point, Vector2 p1, Vector2 p2)
        {
            return Mathf.Abs((p2.x - p1.x) * (p1.y - point.y)) - (p1.x - point.x) * (p2.y - p1.y) /
                   Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
        }

        public static Vector2 RotateVector(Vector2 v, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float _x = (v.x * Mathf.Cos(radian)) - (v.y * Mathf.Sin(radian));
            float _y = (v.x * Mathf.Sin(radian)) + (v.y * Mathf.Cos(radian));
            return new Vector2(_x, _y);
        }

        public static Vector3 RodriguesRotation(Vector3 vectorToRotate, Vector3 dir, float angle)
        {
            Vector3 scaleDown = vectorToRotate * Mathf.Cos(angle);
            Vector3 skew = Vector3.Cross(dir, vectorToRotate) * Mathf.Sin(angle);
            Vector3 height = (1 - Mathf.Cos(angle)) * Vector3.Dot(dir, vectorToRotate) * dir;
            return scaleDown + skew + height;
        }

        public static Vector3 GetCollisionAngle(Collider2D from, Collider2D to) => from && to ? from.bounds.center - to.bounds.center : Vector3.zero;

        public static Vector3 GenerateRandomVector(Vector3 axisToRandomize, float min = 0, float max = 1)
        {
            float x = axisToRandomize.x != 0 ? Random.Range(min, max) : 0;
            float y = axisToRandomize.y != 0 ? Random.Range(min, max) : 0;
            float z = axisToRandomize.z != 0 ? Random.Range(min, max) : 0;

            return new Vector3(x, y, z);
        }

        public static float EqualClamp(float value, float min = 0, float max = 1)
        {
            if (value <= min) value = min;
            else if (value >= max) value = max;
            return value;
        }

        public static void Curve(LineRenderer lineRenderer, float vertexCount, Transform points)
        {
            var pointList = new List<Vector3>();

            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                Vector3 tangent1 = Vector3.Lerp(points.GetChild(0).position, points.GetChild(1).position, ratio);
                Vector3 tangent2 = Vector3.Lerp(points.GetChild(1).position, points.GetChild(2).position, ratio);
                Vector3 curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve);
            }

            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
        }
    }
}
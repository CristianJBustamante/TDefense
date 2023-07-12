using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.Pizia.Tools
{
    public static class PiziaExtensions
    {
        public static bool VerifyName(this string nameToVerify, short minLength, short maxLength, char[] invalidChars = null)
        {
            int length = nameToVerify.Length;
            return length > 0 && length >= minLength && length < maxLength && nameToVerify[0] != ' ' && nameToVerify[length - 1] != ' ' 
                && (invalidChars == null || (invalidChars != null && nameToVerify.All(x => invalidChars.Any(y => x == y))));
        }

        public static void Shuffle<T>(this T[] array)
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

        public static void Shuffle<T>(this List<T> array)
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

        public static string ReplaceChars(this string stringToReplace, char[] charArray)
        {
            int size = stringToReplace.Length;
            if (size == 0) return "";

            string newString = "";
            int lenght = charArray.Length;

            for (int i = 0; i < size; i++)
            {
                bool found = false;
                for (int j = 0; j < lenght; j++)
                {
                    if (newString[i] == charArray[j])
                    {
                        newString += charArray[j];
                        found = true;
                        break;
                    }
                }

                if (!found) newString += stringToReplace[i];
            }

            return newString;
        }

        public static string ReplaceChars(this string stringToReplace, string stringArray)
        {
            int size = stringToReplace.Length;
            if (size == 0) return "";

            string newString = "";
            int lenght = stringArray.Length;

            for (int i = 0; i < size; i++)
            {
                bool found = false;
                for (int j = 0; j < lenght; j++)
                {
                    if (stringToReplace[i] == stringArray[j])
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) newString += stringToReplace[i];
            }

            return newString;
        }

        public static Quaternion QuaterinonClamp(this Quaternion quaternionToClamp, Quaternion axisToClamp, float min = -.5f, float max = .5f)
        {
            float x = axisToClamp.x != 0 ? Mathf.Clamp(quaternionToClamp.x, min, max) : quaternionToClamp.x;
            float y = axisToClamp.y != 0 ? Mathf.Clamp(quaternionToClamp.y, min, max) : quaternionToClamp.y;
            float z = axisToClamp.z != 0 ? Mathf.Clamp(quaternionToClamp.z, min, max) : quaternionToClamp.z;
            float w = axisToClamp.w != 0 ? Mathf.Clamp(quaternionToClamp.w, min, max) : quaternionToClamp.w;

            return new Quaternion(x, y, z, w);
        }

        #region Vector Extensions

        public static Vector3 ClampAngle(this Vector3 anglesToClamp, Vector3 axisToClamp, float min = -10, float max = 10)
        {
            float x = anglesToClamp.x, y = anglesToClamp.y, z = anglesToClamp.z, fixedMin = min < 0 ? 360 - min : min;

            if (axisToClamp.x != 0 && anglesToClamp.x <= fixedMin && anglesToClamp.x >= max)
            {
                float minDiff = Mathf.Abs(anglesToClamp.x - fixedMin);
                float maxDiff = Mathf.Abs(anglesToClamp.x - max);
                if (minDiff < maxDiff) anglesToClamp.x = fixedMin;
                else anglesToClamp.x = max;
            }

            if (axisToClamp.y != 0 && anglesToClamp.y <= fixedMin && anglesToClamp.y >= max)
            {
                float minDiff = Mathf.Abs(anglesToClamp.y - fixedMin);
                float maxDiff = Mathf.Abs(anglesToClamp.y - max);
                if (minDiff < maxDiff) anglesToClamp.y = fixedMin;
                else anglesToClamp.y = max;
            }

            if (axisToClamp.z != 0 && anglesToClamp.z <= fixedMin && anglesToClamp.z >= max)
            {
                float minDiff = Mathf.Abs(anglesToClamp.z - fixedMin);
                float maxDiff = Mathf.Abs(anglesToClamp.z - max);
                if (minDiff < maxDiff) anglesToClamp.z = fixedMin;
                else anglesToClamp.z = max;
            }

            return new Vector3(x, y, z);
        }

        public static Vector3 RandomizeVector(this Vector3 vectorToRandomize, Vector3 axisToRandomize, float min = 0, float max = 1)
        {
            float x = axisToRandomize.x != 0 ? Random.Range(min, max) : vectorToRandomize.x;
            float y = axisToRandomize.y != 0 ? Random.Range(min, max) : vectorToRandomize.y;
            float z = axisToRandomize.z != 0 ? Random.Range(min, max) : vectorToRandomize.z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Clamps a Vector with the given min and max values
        /// </summary>
        /// <param name="axisToClamp">The axis of the vector that can be clamped</param>
        /// <param name="min">The min value to clamp <i>(Default = 0)</i></param>
        /// <param name="max">The max value to clamp <i>(Default = 0)</i></param>
        /// <returns>A new Vector3 clamped in the given values</returns>
        public static Vector3 VectorClamp(this Vector3 vectorToClamp, Vector3 axisToClamp, float min = 0, float max = 1)
        {
            Vector3 newVector = Vector3.zero;

            newVector.x = axisToClamp.x != 0 ? Mathf.Clamp(vectorToClamp.x, min, max) : vectorToClamp.x;
            newVector.y = axisToClamp.y != 0 ? Mathf.Clamp(vectorToClamp.y, min, max) : vectorToClamp.y;
            newVector.z = axisToClamp.z != 0 ? Mathf.Clamp(vectorToClamp.z, min, max) : vectorToClamp.z;

            return newVector;
        }

        /// <summary>
        /// Clamps a Vector with the given min and max values
        /// </summary>
        /// <param name="axisToClamp">The axis of the vector that can be clamped</param>
        /// <param name="min">The min value to clamp <i>(Default = 0)</i></param>
        /// <param name="max">The max value to clamp <i>(Default = 0)</i></param>
        /// <returns>A new Vector2 clamped in the given values</returns>
        public static Vector2 VectorClamp(this Vector2 vectorToClamp, Vector2 axisToClamp, float min = 0, float max = 1)
        {
            Vector2 newVector = Vector2.zero;

            newVector.x = axisToClamp.x != 0 ? Mathf.Clamp(vectorToClamp.x, min, max) : vectorToClamp.x;
            newVector.y = axisToClamp.y != 0 ? Mathf.Clamp(vectorToClamp.y, min, max) : vectorToClamp.y;

            return newVector;
        }

        public static Vector2 VectorEqualClamp(this Vector2 vectorToClamp, Vector2 axisToClamp, float min = 0, float max = 1)
        {
            Vector2 newVector = Vector2.zero;

            newVector.x = axisToClamp.x != 0 ? PiziaUtilities.EqualClamp(vectorToClamp.x, min, max) : vectorToClamp.x;
            newVector.y = axisToClamp.y != 0 ? PiziaUtilities.EqualClamp(vectorToClamp.y, min, max) : vectorToClamp.y;

            return newVector;
        }

        /// <summary>
        /// Clamps a Vector with the given min and max values
        /// </summary>
        /// <param name="axisToClamp">The axis of the vector that can be clamped</param>
        /// <param name="min">The min value to clamp <i>(Default = 0)</i></param>
        /// <param name="max">The max value to clamp <i>(Default = 0)</i></param>
        public static void ClampVector(ref this Vector3 vectorToClamp, Vector3 axisToClamp, float min = 0, float max = 1)
        {
            Vector3 newVector = Vector3.zero;

            newVector.x = axisToClamp.x != 0 ? Mathf.Clamp(vectorToClamp.x, min, max) : vectorToClamp.x;
            newVector.y = axisToClamp.y != 0 ? Mathf.Clamp(vectorToClamp.y, min, max) : vectorToClamp.y;
            newVector.z = axisToClamp.z != 0 ? Mathf.Clamp(vectorToClamp.z, min, max) : vectorToClamp.z;

            vectorToClamp = newVector;
        }

        /// <summary>
        /// Clamps a Vector with the given min and max values
        /// </summary>
        /// <param name="axisToClamp">The axis of the vector that can be clamped</param>
        /// <param name="min">The min value to clamp <i>(Default = 0)</i></param>
        /// <param name="max">The max value to clamp <i>(Default = 0)</i></param>
        public static void ClampVector(ref this Vector2 vectorToClamp, Vector2 axisToClamp, float min = 0, float max = 1)
        {
            Vector2 newVector = Vector2.zero;

            newVector.x = axisToClamp.x != 0 ? Mathf.Clamp(vectorToClamp.x, min, max) : vectorToClamp.x;
            newVector.y = axisToClamp.y != 0 ? Mathf.Clamp(vectorToClamp.y, min, max) : vectorToClamp.y;

            vectorToClamp =  newVector;
        }


        public static Vector3 Direction(this Vector3 vectorA, Vector3 vectorB) => vectorA - vectorB;
        public static Vector2 Direction(this Vector2 vectorA, Vector2 vectorB) => vectorA - vectorB;


        public static Vector3 NormalizedDirection(this Vector3 vectorA, Vector3 vectorB) => Vector3.Normalize(vectorA - vectorB);
        public static Vector2 NormalizedDirection(this Vector2 vectorA, Vector2 vectorB) => (vectorA - vectorB).normalized;


        public static void GetDirection(ref this Vector3 vectorA, Vector3 vectorB) => vectorA = vectorA - vectorB;
        public static void GetDirection(ref this Vector2 vectorA, Vector2 vectorB) => vectorA = vectorA - vectorB;

        public static void GetNormalizedDirection(ref this Vector3 vectorA, Vector3 vectorB) => vectorA = Vector3.Normalize(vectorA - vectorB);
        public static void GetNormalizedDirection(ref this Vector2 vectorA, Vector2 vectorB) => vectorA = (vectorA - vectorB).normalized;

        public static bool VectorsEqualIgnoreY(ref this Vector3 vectorA, Vector3 vectorB) => vectorA.x == vectorB.x && vectorA.z == vectorB.z;

        public static bool VectorsEqual(ref this Vector3 vectorA, Vector3 vectorB, float tolerance = .01f)
        {
            Vector3 diff = vectorA - vectorB;
            return Mathf.Abs(diff.x) <= tolerance && Mathf.Abs(diff.y) <= tolerance && Mathf.Abs(diff.z) <= tolerance;
        }

        public static bool VectorsEqual(ref this Vector2 vectorA, Vector2 vectorB, float tolerance = .01f)
        {
            Vector2 diff = vectorA - vectorB;
            return Mathf.Abs(diff.x) <= tolerance && Mathf.Abs(diff.y) <= tolerance;
        }

        #endregion
    }
}
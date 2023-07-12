using System.Collections.Generic;
using UnityEngine;

namespace com.Pizia.Tools
{
    public class ObjectPool<T> where T : Component
    {
        class PoolObject
        {
            public readonly GameObject poolGameObject;
            public readonly T poolScript;

            public PoolObject(GameObject poolObject, T _PoolScript)
            {
                poolGameObject = poolObject;
                poolScript = _PoolScript;
            }
        }

        public GameObject objectPrefab;

        public int Count => pool.Count;

        int _LastActivesCount;
        int _Actives;
        public int ActiveCount
        {
            get
            {
                int count = pool.Count;
                if (_LastActivesCount != count)
                {
                    _Actives = 0;
                    for (int i = 0; i < count; i++)
                        _Actives += pool[i].poolGameObject.activeSelf ? 1 : 0;
                }

                return _Actives;
            }
        }

        readonly Transform objectParent;
        readonly Vector3 startPos;

        readonly List<PoolObject> pool;

        int nextIndx;

        public ObjectPool(GameObject _ObjectPrefab, Transform _ObjectParent = null, int startQuantity = 0)
        {
            objectPrefab = _ObjectPrefab;
            objectParent = _ObjectParent;
            startPos = Vector3.zero;

            pool = new List<PoolObject>();
            for (int i = 0; i < startQuantity; i++) AddNewObjectToPool();
        }

        public ObjectPool(GameObject _ObjectPrefab, Vector3 spawnPosition, int startQuantity = 0)
        {
            objectPrefab = _ObjectPrefab;
            startPos = spawnPosition;

            pool = new List<PoolObject>();
            for (int i = 0; i < startQuantity; i++) AddNewObjectToPool();
        }

        void AddNewObjectToPool()
        {
            GameObject newObject = GameObject.Instantiate(objectPrefab, startPos, Quaternion.identity, objectParent);
            pool.Add(new PoolObject(newObject, newObject.GetComponent<T>()));
        }

        public GameObject GetGameObjectByIndex(int index)
        {
            if (index >= pool.Count) AddNewObjectToPool();
            else pool[index].poolGameObject.SetActive(true);

            return pool[index].poolGameObject;
        }

        public T GetByIndex(int index)
        {
            if (index >= pool.Count) AddNewObjectToPool();
            else pool[index].poolGameObject.SetActive(true);

            return pool[index].poolScript;
        }

        #region GetNext

        public GameObject GetNextGameObject()
        {
            if (nextIndx >= Count) nextIndx = 0;
            return pool[nextIndx++].poolGameObject;
        }

        public T GetNext()
        {
            if (nextIndx >= Count) nextIndx = 0;
            return pool[nextIndx++].poolScript;
        }

        public GameObject GetNextGameObjectActive(System.Func<GameObject, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolGameObject))
                    return pool[i].poolGameObject;
            }

            return default;
        }

        public GameObject GetNextGameObjectActive(System.Func<T, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolScript))
                    return pool[i].poolGameObject;
            }

            return default;
        }

        public GameObject GetNextGameObjectActive()
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].poolGameObject.activeSelf)
                    return pool[i].poolGameObject;
            }

            return default;
        }

        public T GetNextActive(System.Func<GameObject, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolGameObject))
                    return pool[i].poolScript;
            }

            return default;
        }

        public T GetNextActive(System.Func<T, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].poolGameObject.activeSelf && 
                    condition != null && condition.Invoke(pool[i].poolScript))
                    return pool[i].poolScript;
            }

            return default;
        }

        public T GetNextActive()
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].poolGameObject.activeSelf)
                    return pool[i].poolScript;
            }

            return default;
        }

        public GameObject GetNextGameObjectNotActive(System.Func<GameObject, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolGameObject))
                    return pool[i].poolGameObject;
            }

            return default;
        }

        public GameObject GetNextGameObjectNotActive(System.Func<T, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolScript))
                    return pool[i].poolGameObject;
            }

            return default;
        }

        public GameObject GetNextGameObjectNotActive()
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].poolGameObject.activeSelf)
                    return pool[i].poolGameObject;
            }

            return default;
        }

        public T GetNextNotActive(System.Func<GameObject, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolGameObject))
                    return pool[i].poolScript;
            }

            return default;
        }

        public T GetNextNotActive(System.Func<T, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].poolGameObject.activeSelf &&
                    condition != null && condition.Invoke(pool[i].poolScript))
                    return pool[i].poolScript;
            }

            return default;
        }

        public T GetNextNotActive()
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].poolGameObject.activeSelf)
                    return pool[i].poolScript;
            }

            return default;
        }

        #endregion

        public void ToggleByIndex(int index, bool toggle)
        {
            if (index >= pool.Count) return;
            pool[index].poolGameObject.SetActive(toggle);
        }

        public void ToggleAll(bool activate)
        {
            int size = pool.Count;
            for (int i = 0; i < size; i++) pool[i].poolGameObject.SetActive(activate);
        }

        #region AddColection

        public void AddToPool(List<GameObject> objectList)
        {
            int size = objectList.Count;
            for (int i = 0; i < size; i++)
                pool.Add(new PoolObject(objectList[i], objectList[i].GetComponent<T>()));
        }

        public void AddToPool(GameObject[] objectArray)
        {
            int size = objectArray.Length;
            for (int i = 0; i < size; i++)
                pool.Add(new PoolObject(objectArray[i], objectArray[i].GetComponent<T>()));
        }

        public void AddToPool(List<T> objectScriptList)
        {
            int size = objectScriptList.Count;
            for (int i = 0; i < size; i++)
                pool.Add(new PoolObject(objectScriptList[i].gameObject, objectScriptList[i]));
        }

        public void AddToPool(T[] objectScriptArray)
        {
            int size = objectScriptArray.Length;
            for (int i = 0; i < size; i++)
                pool.Add(new PoolObject(objectScriptArray[i].gameObject, objectScriptArray[i]));
        }

        #endregion

        #region Get Array/List

        public T[] GetArray()
        {
            int count = Count;
            T[] objectsArray = new T[count];

            for (int i = 0; i < count; i++)
                objectsArray[i] = pool[i].poolScript;

            return objectsArray;
        }

        public T[] GetActiveArray()
        {
            int count = Count;
            T[] objectsArray = new T[ActiveCount];

            for (int i = 0; i < count; i++)
            {
                if (pool[i].poolGameObject.activeSelf)
                    objectsArray[i] = pool[i].poolScript;
            }

            return objectsArray;
        }

        public GameObject[] GetGameObjectArray()
        {
            int count = Count;
            GameObject[] objectsArray = new GameObject[count];

            for (int i = 0; i < count; i++)
                objectsArray[i] = pool[i].poolGameObject;

            return objectsArray;
        }

        public GameObject[] GetActiveGameObjectArray()
        {
            int count = Count;
            GameObject[] objectsArray = new GameObject[ActiveCount];

            for (int i = 0; i < count; i++)
            {
                if (pool[i].poolGameObject.activeSelf)
                    objectsArray[i] = pool[i].poolGameObject;
            }

            return objectsArray;
        }

        public List<T> GetList()
        {
            int count = Count;
            List<T> objectsList = new List<T>();

            for (int i = 0; i < count; i++)
                objectsList.Add(pool[i].poolScript);

            return objectsList;
        }

        public List<T> GetActiveList()
        {
            int count = Count;
            List<T> objectsList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                if (pool[i].poolGameObject.activeSelf)
                    objectsList.Add(pool[i].poolScript);
            }

            return objectsList;
        }

        public List<GameObject> GetGameObjectList()
        {
            int count = Count;
            List<GameObject> objectsList = new List<GameObject>();

            for (int i = 0; i < count; i++)
                objectsList.Add(pool[i].poolGameObject);

            return objectsList;
        }

        public List<GameObject> GetActiveGameObjectList()
        {
            int count = Count;
            List<GameObject> objectsList = new List<GameObject>();

            for (int i = 0; i < count; i++)
            {
                if (pool[i].poolGameObject.activeSelf)
                    objectsList.Add(pool[i].poolGameObject);
            }

            return objectsList;
        }

        #endregion
    }

    public class GameObjectPool
    {
        public GameObject objectPrefab;

        public int Count => pool.Count;

        int _LastActivesCount;
        int _Actives;
        public int ActiveCount
        {
            get
            {
                int count = pool.Count;
                if (_LastActivesCount != count)
                {
                    _Actives = 0;
                    for (int i = 0; i < count; i++)
                        _Actives += pool[i].activeSelf ? 1 : 0;
                }

                return _Actives;
            }
        }

        readonly Transform objectParent;
        readonly Vector3 startPos;

        readonly List<GameObject> pool;

        int nextIndx;

        public GameObjectPool(GameObject _ObjectPrefab, Transform _ObjectParent = null, int startQuantity = 0)
        {
            objectPrefab = _ObjectPrefab;
            objectParent = _ObjectParent;
            startPos = Vector3.zero;

            pool = new List<GameObject>();
            for (int i = 0; i < startQuantity; i++) AddNewObjectToPool();
        }

        public GameObjectPool(GameObject _ObjectPrefab, Vector3 spawnPosition, int startQuantity = 0)
        {
            objectPrefab = _ObjectPrefab;
            startPos = spawnPosition;

            pool = new List<GameObject>();
            for (int i = 0; i < startQuantity; i++) AddNewObjectToPool();
        }

        void AddNewObjectToPool() => pool.Add(GameObject.Instantiate(objectPrefab, startPos, Quaternion.identity, objectParent));

        public GameObject GetByIndex(int index)
        {
            if (index >= pool.Count) AddNewObjectToPool();
            else pool[index].SetActive(true);

            return pool[index];
        }

        public GameObject GetNext()
        {
            if (nextIndx >= Count) nextIndx = 0;
            return pool[nextIndx++];
        }

        public GameObject GetNextActive(System.Func<GameObject, bool> condition)
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].activeSelf &&
                    condition != null && condition.Invoke(pool[i]))
                    return pool[i];
            }

            return GetByIndex(size);
        }

        public GameObject GetNextActive()
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (pool[i].activeSelf)
                    return pool[i];
            }

            return GetByIndex(size);
        }

        public GameObject GetNextNotActive()
        {
            int size = Count;
            for (int i = 0; i < size; i++)
            {
                if (!pool[i].activeSelf)
                    return pool[i];
            }

            return GetByIndex(size);
        }

        public void ToggleByIndex(int index, bool toggle)
        {
            if (index >= pool.Count) return;
            pool[index].SetActive(toggle);
        }

        public void ToggleAll(bool activate)
        {
            int size = pool.Count;
            for (int i = 0; i < size; i++) pool[i].SetActive(activate);
        }

        public void AddToPool(List<GameObject> objectList)
        {
            int size = objectList.Count;
            for (int i = 0; i < size; i++)
                pool.Add(objectList[i]);
        }

        public void AddToPool(GameObject[] objectArray)
        {
            int size = objectArray.Length;
            for (int i = 0; i < size; i++)
                pool.Add(objectArray[i]);
        }

        public GameObject[] GetGameObjectArray() => pool.ToArray();

        public GameObject[] GetActiveGameObjectArray() => pool.FindAll(x => x.activeSelf).ToArray();

        public List<GameObject> GetGameObjectList() => pool;

        public List<GameObject> GetActiveGameObjectList() => pool.FindAll(x => x.activeSelf);
    }
}

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public interface IPoolObject
{
    /// <summary>
    /// 생성된 오브젝트 초기 세팅
    /// </summary>
    /// <param name="InstancePoint">생성 포인트</param>
    public void SetObject(IObjectPooler objectPooler, Transform InstancePoint);

    public void SetTransform(Transform InstancePoint);

    /// <summary>
    /// 오브젝트 활성화
    /// </summary>
    /// <param name="InstancePoint">활성화 위치</param>
    public GameObject ActiveObject(Transform InstancePoint);

    /// <summary>
    /// 비활성화 후 풀에 오브젝트 반환
    /// </summary>
    public void DeactiveObject();
}

public interface IObjectPooler
{
    /// <summary>
    /// 풀러에 ObjectPoolManager 등록
    /// </summary>
    /// <param name="objectPoolManager">등록할 ObjectPoolManager</param>
    public void SetPooler(ObjectPoolManager objectPoolManager);

    /// <summary>
    /// 풀러 정보 얻기
    /// </summary>
    /// <returns>풀러의 정보</returns>
    public ObjectPoolerExtension GetPoolerInfo();

    /// <summary>
    /// 풀링 오브젝트 활성화
    /// </summary>
    public void ActiveObject();

    /// <summary>
    /// 풀링 오브젝트 비활성화
    /// </summary>
    public void DeactiveObject(IPoolObject poolObject);
}

[System.Serializable]
public class ObjectPoolerInfo
{
    public string poolerName;
    public GameObject InstanceObject;
    public Transform InstancePoint;
    public int generateCount;
}

[System.Serializable]
public class ObjectPoolerExtension : ObjectPoolerInfo
{
    public List<IPoolObject> deactiveObjects = new List<IPoolObject>();
    public List<IPoolObject> activeObjects = new List<IPoolObject>();
    public Transform poolFolder;
}

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectPoolers = new List<GameObject>();

    private Dictionary<IObjectPooler, ObjectPoolerExtension> objectPoolers = new Dictionary<IObjectPooler, ObjectPoolerExtension>();

    void Awake()
    {
        for (int i = 0; i < _objectPoolers.Count; i++)
        {
            IObjectPooler objectPooler = _objectPoolers[i].GetComponent<IObjectPooler>();
            objectPooler.SetPooler(this);

            ObjectPoolerExtension objectPoolerExtension = objectPooler.GetPoolerInfo();

            Transform poolerFolder = new GameObject($"{i}_{objectPoolerExtension.poolerName}").transform;
            poolerFolder.parent = this.transform;

            objectPoolerExtension.poolFolder = poolerFolder;

            this.objectPoolers.Add(objectPooler, objectPoolerExtension);
            GeneratePool(objectPooler, objectPoolerExtension);
        }
    }

    /// <summary>
    /// GenerateCount 만큼 풀링 오브젝트 생성후 DeactiveObjects에 등록
    /// </summary>
    public void GeneratePool(IObjectPooler objectPooler, ObjectPoolerExtension objectPoolerExtension)
    {
        for (int i = 0; i < objectPoolerExtension.generateCount; i++)
        {
            GameObject instanceObject = Instantiate(objectPoolerExtension.InstanceObject, objectPoolerExtension.poolFolder);
            IPoolObject poolObject = instanceObject.GetComponent<IPoolObject>();
            poolObject.SetObject(objectPooler, objectPoolerExtension.InstancePoint);
            objectPoolerExtension.deactiveObjects.Add(poolObject);
        }
    }

    /// <summary>
    /// 풀에서 오브젝트 활성화
    /// </summary>
    /// <param name="objectPooler">오브젝트 풀러</param>
    public GameObject ActiveObject(IObjectPooler objectPooler)
    {
        ObjectPoolerExtension objectPoolerExtension = objectPoolers[objectPooler];
        if(objectPoolerExtension.deactiveObjects.Count == 0)
        {
            GeneratePool(objectPooler, objectPoolerExtension);
        }

        IPoolObject poolObject = objectPoolerExtension.deactiveObjects[0];
        GameObject gameObject = poolObject.ActiveObject(objectPoolerExtension.InstancePoint);

        objectPoolerExtension.deactiveObjects.Remove(poolObject);
        objectPoolerExtension.activeObjects.Add(poolObject);

        return gameObject;
    }

    /// <summary>
    /// 풀에서 오브젝트 비활성화
    /// </summary>
    /// <param name="objectPooler"></param>
    public void DeactiveObject(IObjectPooler objectPooler, IPoolObject poolObject)
    {
        ObjectPoolerExtension objectPoolerExtension = objectPoolers[objectPooler];
        objectPoolerExtension.activeObjects.Remove(poolObject);
        objectPoolerExtension.deactiveObjects.Add(poolObject);
    }
}

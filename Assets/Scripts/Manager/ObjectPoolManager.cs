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
    public string poolerName; // 오브젝트 풀러의 이름
    public GameObject InstanceObject; // 풀링할 오브젝트 위치
    public Transform InstancePoint; // 풀링할 오브젝트의 초기 위치
    public int generateCount; // 풀링 오브젝트 생성 개수
}

[System.Serializable]
public class ObjectPoolerExtension : ObjectPoolerInfo
{
    public List<IPoolObject> deactiveObjects = new List<IPoolObject>(); //비활성화 되있는 오브젝트 리스트
    public List<IPoolObject> activeObjects = new List<IPoolObject>(); // 활성화 되있는 오브젝트 리스트

    [HideInInspector]
    public Transform poolFolder; // 풀링 오브젝트들의 루트 폴터
}

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<IObjectPooler, ObjectPoolerExtension> objectPoolers = new Dictionary<IObjectPooler, ObjectPoolerExtension>();

    void Awake()
    {
        // 현재 씬에 IObjectPooler 인터페이스를 가지고 있는 모든 오브젝트를 탐색하여 가져옴
        IObjectPooler[] objectPoolers = FindObjectsOfType<MonoBehaviour>().OfType<IObjectPooler>().ToArray();

        for (int i = 0; i < objectPoolers.Length; i++)
        {
            var objectPooler = objectPoolers[i];

            // 풀러에 이 매니저를 등록함
            objectPooler.SetPooler(this);

            // ObjectPooler의 정보를 가져옴
            ObjectPoolerExtension objectPoolerExtension = objectPooler.GetPoolerInfo();

            // 풀링 오브젝트의 루트 오브젝트를 생성함
            Transform poolerFolder = new GameObject($"{i}_{objectPoolerExtension.poolerName}").transform;
            poolerFolder.parent = this.transform;

            // 오브젝트 풀러의 poolFolder를 위에 생성한 poolerFolder로 설정함
            objectPoolerExtension.poolFolder = poolerFolder;

            // 오브젝트 풀러를 등록함 
            this.objectPoolers.Add(objectPooler, objectPoolerExtension);
            
            // 풀링 오브젝트를 생성하고 등록함
            GeneratePoolObject(objectPooler, objectPoolerExtension);
        }
    }

    /// <summary>
    /// GenerateCount 만큼 풀링 오브젝트 생성후 DeactiveObjects에 등록
    /// </summary>
    public void GeneratePoolObject(IObjectPooler objectPooler, ObjectPoolerExtension objectPoolerExtension)
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
            GeneratePoolObject(objectPooler, objectPoolerExtension);
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

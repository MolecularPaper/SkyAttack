using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public interface IPoolObject
{
    /// <summary>
    /// ������ ������Ʈ �ʱ� ����
    /// </summary>
    /// <param name="InstancePoint">���� ����Ʈ</param>
    public void SetObject(IObjectPooler objectPooler, Transform InstancePoint);

    public void SetTransform(Transform InstancePoint);

    /// <summary>
    /// ������Ʈ Ȱ��ȭ
    /// </summary>
    /// <param name="InstancePoint">Ȱ��ȭ ��ġ</param>
    public GameObject ActiveObject(Transform InstancePoint);

    /// <summary>
    /// ��Ȱ��ȭ �� Ǯ�� ������Ʈ ��ȯ
    /// </summary>
    public void DeactiveObject();
}

public interface IObjectPooler
{
    /// <summary>
    /// Ǯ���� ObjectPoolManager ���
    /// </summary>
    /// <param name="objectPoolManager">����� ObjectPoolManager</param>
    public void SetPooler(ObjectPoolManager objectPoolManager);

    /// <summary>
    /// Ǯ�� ���� ���
    /// </summary>
    /// <returns>Ǯ���� ����</returns>
    public ObjectPoolerExtension GetPoolerInfo();

    /// <summary>
    /// Ǯ�� ������Ʈ Ȱ��ȭ
    /// </summary>
    public void ActiveObject();

    /// <summary>
    /// Ǯ�� ������Ʈ ��Ȱ��ȭ
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
    /// GenerateCount ��ŭ Ǯ�� ������Ʈ ������ DeactiveObjects�� ���
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
    /// Ǯ���� ������Ʈ Ȱ��ȭ
    /// </summary>
    /// <param name="objectPooler">������Ʈ Ǯ��</param>
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
    /// Ǯ���� ������Ʈ ��Ȱ��ȭ
    /// </summary>
    /// <param name="objectPooler"></param>
    public void DeactiveObject(IObjectPooler objectPooler, IPoolObject poolObject)
    {
        ObjectPoolerExtension objectPoolerExtension = objectPoolers[objectPooler];
        objectPoolerExtension.activeObjects.Remove(poolObject);
        objectPoolerExtension.deactiveObjects.Add(poolObject);
    }
}

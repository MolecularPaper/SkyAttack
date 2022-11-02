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
    public string poolerName; // ������Ʈ Ǯ���� �̸�
    public GameObject InstanceObject; // Ǯ���� ������Ʈ ��ġ
    public Transform InstancePoint; // Ǯ���� ������Ʈ�� �ʱ� ��ġ
    public int generateCount; // Ǯ�� ������Ʈ ���� ����
}

[System.Serializable]
public class ObjectPoolerExtension : ObjectPoolerInfo
{
    public List<IPoolObject> deactiveObjects = new List<IPoolObject>(); //��Ȱ��ȭ ���ִ� ������Ʈ ����Ʈ
    public List<IPoolObject> activeObjects = new List<IPoolObject>(); // Ȱ��ȭ ���ִ� ������Ʈ ����Ʈ

    [HideInInspector]
    public Transform poolFolder; // Ǯ�� ������Ʈ���� ��Ʈ ����
}

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<IObjectPooler, ObjectPoolerExtension> objectPoolers = new Dictionary<IObjectPooler, ObjectPoolerExtension>();

    void Awake()
    {
        // ���� ���� IObjectPooler �������̽��� ������ �ִ� ��� ������Ʈ�� Ž���Ͽ� ������
        IObjectPooler[] objectPoolers = FindObjectsOfType<MonoBehaviour>().OfType<IObjectPooler>().ToArray();

        for (int i = 0; i < objectPoolers.Length; i++)
        {
            var objectPooler = objectPoolers[i];

            // Ǯ���� �� �Ŵ����� �����
            objectPooler.SetPooler(this);

            // ObjectPooler�� ������ ������
            ObjectPoolerExtension objectPoolerExtension = objectPooler.GetPoolerInfo();

            // Ǯ�� ������Ʈ�� ��Ʈ ������Ʈ�� ������
            Transform poolerFolder = new GameObject($"{i}_{objectPoolerExtension.poolerName}").transform;
            poolerFolder.parent = this.transform;

            // ������Ʈ Ǯ���� poolFolder�� ���� ������ poolerFolder�� ������
            objectPoolerExtension.poolFolder = poolerFolder;

            // ������Ʈ Ǯ���� ����� 
            this.objectPoolers.Add(objectPooler, objectPoolerExtension);
            
            // Ǯ�� ������Ʈ�� �����ϰ� �����
            GeneratePoolObject(objectPooler, objectPoolerExtension);
        }
    }

    /// <summary>
    /// GenerateCount ��ŭ Ǯ�� ������Ʈ ������ DeactiveObjects�� ���
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
    /// Ǯ���� ������Ʈ Ȱ��ȭ
    /// </summary>
    /// <param name="objectPooler">������Ʈ Ǯ��</param>
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

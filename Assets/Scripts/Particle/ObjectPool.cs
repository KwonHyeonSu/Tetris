using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject Particle_BlockDown;
    [SerializeField]
    private GameObject Particle_BlockBoom;

    Queue<Particle_BlockDown> BlockBoom_Queue = new Queue<Particle_BlockDown>();
    Queue<Particle_BlockDown> BlockDown_Queue = new Queue<Particle_BlockDown>();

    private void Awake()
    {
        Instance = this;

        Init_BlockDown(5);
        Init_BlockBoom(5);
    }

    void Init_BlockDown(int initCount)
    {
        for(int i=0;i < initCount; i++)
        {
            BlockDown_Queue.Enqueue(CreateNewObject("BlockDown"));
        }
    }

    void Init_BlockBoom(int initCount)
    {
        for(int i=0;i < initCount; i++)
        {
            BlockBoom_Queue.Enqueue(CreateNewObject("BlockBoom"));
        }
    }

    private Particle_BlockDown CreateNewObject(string particleName)
    {
        if(particleName == "BlockDown")
        {
            Particle_BlockDown newObj = Instantiate(Particle_BlockDown).GetComponent<Particle_BlockDown>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        if(particleName == "BlockBoom")
        {
            Particle_BlockDown newObj = Instantiate(Particle_BlockBoom).GetComponent<Particle_BlockDown>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        return null;
    }

    public static Particle_BlockDown GetObject(string particle_Name, Vector3 position)
    {

        if(particle_Name == "BlockDown")
        {
            if(Instance.BlockDown_Queue.Count > 0)
            {

                var obj = Instance.BlockDown_Queue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                obj.transform.localPosition = position;
                return obj;
                
            }

            else{
                var newObj = Instance.CreateNewObject("BlockDown");
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                newObj.transform.localPosition = position;
                return newObj;
            }
            
        }

        else if(particle_Name == "BlockBoom")
        {
            if(Instance.BlockBoom_Queue.Count > 0)
            {

                var obj = Instance.BlockBoom_Queue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                obj.transform.localPosition = position;
                return obj;
                
            }

            else{
                var newObj = Instance.CreateNewObject("BlockBoom");
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                newObj.transform.localPosition = position;
                return newObj;
            }

        }
        return null;
        
    }

    public static void ReturnObject(Particle_BlockDown obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.BlockDown_Queue.Enqueue(obj);
    }
}

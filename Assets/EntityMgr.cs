using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public static EntityMgr inst;
    private void Awake()
    {
        inst = this;
        entities = new List<Entity>();
        foreach(Entity ent in movableEntitiesRoot.GetComponentsInChildren<Entity>()) {
            entities.Add(ent);
        }
    }

    public GameObject movableEntitiesRoot;
    public List<GameObject> entityPrefabs;
    public GameObject entitiesRoot;
    public List<Entity> entities;

    public static int entityId = 0;

    public Entity CreateEntity(EntityType et, Vector3 position, Vector3 eulerAngles)
    {
        Entity entity = null;
        GameObject entityPrefab = entityPrefabs.Find(x => (x.GetComponent<Entity>().entityType == et));
        if (entityPrefab != null) {
            GameObject entityGo = Instantiate(entityPrefab, position, Quaternion.Euler(eulerAngles), entitiesRoot.transform);
            if (entityGo != null) {
                entity = entityGo.GetComponent<Entity>();
                entityGo.name = et.ToString() + entityId++;
                entities.Add(entity);
            }
        }
        return entity;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

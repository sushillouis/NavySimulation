using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public static EntityMgr inst;
    private void Awake()
    {
        inst = this;
        entities = new List<Entity381>();
        foreach(Entity381 ent in movableEntitiesRoot.GetComponentsInChildren<Entity381>()) {
            entities.Add(ent);
        }
    }

    public GameObject movableEntitiesRoot;
    public List<GameObject> entityPrefabs;
    public GameObject entitiesRoot;
    public List<Entity381> entities;

    public static int entityId = 0;

    public Entity381 CreateEntity(EntityType et, Vector3 position, Vector3 eulerAngles)
    {
        Entity381 entity = null;
        GameObject entityPrefab = entityPrefabs.Find(x => (x.GetComponent<Entity381>().entityType == et));
        if (entityPrefab != null) {
            GameObject entityGo = Instantiate(entityPrefab, position, Quaternion.Euler(eulerAngles), entitiesRoot.transform);
            if (entityGo != null) {
                entity = entityGo.GetComponent<Entity381>();
                entityGo.name = et.ToString() + entityId++;
                entities.Add(entity);
            }
        }

        DistanceMgr.inst.Initialize();
        VOMgr.inst.Initialize();

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

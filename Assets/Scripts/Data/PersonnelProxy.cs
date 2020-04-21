
using Types;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PersonnelProxy))]
public class PersonnelProxy : MonoBehaviour
{
    public float Morale = 600.0f;
    public float Fitness = 14400.0f;
    public uint Count = 12;
    public uint Wounded = 0;
    public uint Killed = 0;
    
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
            if (thisPersonnelId == 0)
            {
                thisPersonnelId = _entityAssembly.RegisterPersonnelProxy(this);
                _entityProxy.personnelId = thisPersonnelId;
                entityId = _entityProxy.entityId;
                morale = Morale;
                fitness = Fitness;
                count = Count;
                wounded = Wounded;
                killed = Killed;
            }
        }    
    }

    void OnDestroy()
    {
        _entityProxy.personnelId = 0;
    }
    
    public uint entityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                return thisPersonnel.entityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                thisPersonnel.entityId = value;
                _entityAssembly.SetPersonnel(thisPersonnelId, thisPersonnel);
            }
        }
    }

    public float morale
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                return thisPersonnel.morale;
            }
            else
            {
                return 0.0f;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                thisPersonnel.morale = value;
                _entityAssembly.SetPersonnel(thisPersonnelId, thisPersonnel);
            }
        }
    }
    
    public float fitness
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                return thisPersonnel.fitness;
            }
            else
            {
                return 0.0f;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                thisPersonnel.fitness = value;
                _entityAssembly.SetPersonnel(thisPersonnelId, thisPersonnel);
            }
        }
    }
    
    public uint count
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                return thisPersonnel.count;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                thisPersonnel.count = value;
                _entityAssembly.SetPersonnel(thisPersonnelId, thisPersonnel);
            }
        }
    }
    
    public uint wounded
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                return thisPersonnel.wounded;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                thisPersonnel.wounded = value;
                _entityAssembly.SetPersonnel(thisPersonnelId, thisPersonnel);
            }
        }
    }
    
    public uint killed
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                return thisPersonnel.killed;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisPersonnelId = _entityAssembly.GetPersonnelId(this);
                Structs.Personnel thisPersonnel = _entityAssembly.GetPersonnel(thisPersonnelId);
                thisPersonnel.killed = value;
                _entityAssembly.SetPersonnel(thisPersonnelId, thisPersonnel);
            }
        }
    }
}

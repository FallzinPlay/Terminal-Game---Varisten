using Game;
using System;
using System.Collections.Generic;

public class EntityRegistry
{
    // Usando um dicionário para armazenar qualquer tipo de entidade que derive de Identifier
    private Dictionary<Guid, Identifier> Entities = new Dictionary<Guid, Identifier>();

    // Método genérico para adicionar qualquer entidade que derive de Identifier
    public void AddEntity(params Identifier[] entity)
    {
        foreach (Identifier en in entity)
        {
            if (en != null)
                Entities[en.Id] = en;
        }
    }

    // Método genérico para recuperar qualquer entidade que derive de Identifier
    public T GetEntityById<T>(Guid id) where T : Identifier
    {
        Entities.TryGetValue(id, out var entity);
        return entity as T;
    }

    public Dictionary<Guid, Identifier> GetAllEntities()
    {
        return Entities;
    }
}

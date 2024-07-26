using Game;
using System;
using System.Collections.Generic;

public class EntityRegistry
{
    // Usando um dicionário para armazenar qualquer tipo de entidade que derive de Identifier
    private readonly Dictionary<Guid, Identifier> _entities = new Dictionary<Guid, Identifier>();

    // Método genérico para adicionar qualquer entidade que derive de Identifier
    public void AddEntity(Identifier entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _entities[entity.Id] = entity;
    }

    // Método genérico para recuperar qualquer entidade que derive de Identifier
    public T GetEntityById<T>(Guid id) where T : Identifier
    {
        _entities.TryGetValue(id, out var entity);
        return entity as T;
    }
}

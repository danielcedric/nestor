using System;

namespace Nestor.Tools.Domain.Entities
{
    public interface IEntity
    {
        ///// <summary>
        ///// Recopie l'ensemble des propriétés simples (membres par valeur).
        ///// Ne recopie par les champs qui composent la clé unique ainsi que l'identifiant.
        ///// </summary>
        ///// <param name="other">objet à recopier</param>
        ///// <returns>Copie</returns>
        void CopyFrom(IEntity other);
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
    }

    public interface IEntityWithComposedPrimaryKey : IEntity
    {
        
    }

    public interface IEntityWithId: IEntity<Guid> { }

}
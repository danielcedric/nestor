using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.Exceptions
{
    public class ParentIdNotCorrespondsChildId : NestorException
    {
        public ParentIdNotCorrespondsChildId(string entityName) : base($"{entityName} identifiers do not correspond.")
        {
        }

        public ParentIdNotCorrespondsChildId(long parentId, string parentName, long childId, string childName) : base(
            $"The first given Id ({parentId}) for {parentName} does not correspond to the {childName} identifier ({childId})")
        {
        }
    }
}
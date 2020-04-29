namespace Nestor.Tools.Exceptions
{
    public class MajorMinorPatchIdentifiersAreRequiredException : NestorException
    {
        public MajorMinorPatchIdentifiersAreRequiredException() : base("Major, minor and patch identifiers are required")
        {
        }
    }
}

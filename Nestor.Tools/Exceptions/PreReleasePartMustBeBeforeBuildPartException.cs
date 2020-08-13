namespace Nestor.Tools.Exceptions
{
    public class PreReleasePartMustBeBeforeBuildPartException : NestorException
    {
        public PreReleasePartMustBeBeforeBuildPartException() : base(
            "The pre-release part must be before the build part")
        {
        }
    }
}
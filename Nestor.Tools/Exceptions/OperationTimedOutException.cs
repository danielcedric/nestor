namespace Nestor.Tools.Exceptions
{
    public class OperationTimedOutException : NestorException
    {
        public OperationTimedOutException() : base("The time allotted for the operation has expired")
        {
        }
    }
}
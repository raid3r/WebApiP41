namespace WebApiP41.ErrorHandling;

public class OrderNotAcceptedByTimeException : Exception
{
    public OrderNotAcceptedByTimeException(string message, int startHour, int endHour) : base(message)
    {
        StartHour = startHour;
        EndHour = endHour;
    }
    public int StartHour { get; private set; }
    public int EndHour { get; private set; }
}

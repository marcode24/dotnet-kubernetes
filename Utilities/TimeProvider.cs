namespace NetKubernetes.Utilities;

public class TimeProvider : ITimeProvider
{
  public DateTime Now => DateTime.Now;
}

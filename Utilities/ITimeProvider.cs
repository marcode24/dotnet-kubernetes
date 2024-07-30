namespace NetKubernetes.Utilities;

public interface ITimeProvider
{
  DateTime Now { get; }
}
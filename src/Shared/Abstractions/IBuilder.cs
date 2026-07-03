namespace Shared.Abstractions;

public interface IBuilder<T>
{
    T Build();
    bool CanBuild { get; }
}

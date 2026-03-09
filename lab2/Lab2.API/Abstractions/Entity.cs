namespace Lab2.Abstractions;

public abstract class Entity<TId>
    where TId : struct
{
    public TId Id { get; set; }
}
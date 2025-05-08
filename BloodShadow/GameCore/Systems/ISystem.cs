namespace BloodShadow.GameCore.Systems
{
    public interface ISystem : IStartSystem, IUpdateSystem, IFixedUpdateSystem { }
    public interface IStartSystem { void Start(); }
    public interface IUpdateSystem { void Update(); }
    public interface IFixedUpdateSystem { void FixedUpdate(); }
}

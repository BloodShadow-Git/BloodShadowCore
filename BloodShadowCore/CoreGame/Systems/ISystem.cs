namespace BloodShadow.CoreGame.Systems
{
    public interface ISystem : IStartSystem, IUpdateSystem, IFixedUpdateSystem { }
    public interface IStartSystem { void Start(); }
    public interface IUpdateSystem { void Update(in float delta); }
    public interface IFixedUpdateSystem { void FixedUpdate(in float delta); }
}

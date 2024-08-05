namespace JRPG.Abstract
{
    public interface IQuest
    {
        string Description { get; }
        int Experience { get; }
        IItem Requirement { get; }
        IItem Reward { get; }
        string Title { get; }
    }
}
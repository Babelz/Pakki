namespace Inviscan.Sync.Services
{
    public enum InventoryEvent : byte
    {
        QuantityChanged,
        BoxGradeChanged,
        GameGradeChanged,
        ManualGradeChanged,
        NoteChanged
    }

    public interface ICollectionLogService
    {
    }

    public class CollectionLogService : ICollectionLogService
    {
    }
}
namespace Inviscan.Sync.Services
{
    public enum InventoryEvent : byte
    {
        AddStock,
        RemoveStock,
        ChangeBoxGrade,
        ChangeGameGrade,
        ChangeManualGrade,
        ChangeNote,
        Created,
        InitialStockInserted,
        OutOfStock
    }
    
    public interface ICollectionLogService
    {
    }

    public class CollectionLogService : ICollectionLogService
    {
    }
}
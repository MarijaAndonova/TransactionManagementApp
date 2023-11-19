namespace MvcCore.BussinesLogic
{
    public static class Enums
    {
        [Flags]
        public enum TransactionType
        {
            Debit,
            Credit
        }
    }
}

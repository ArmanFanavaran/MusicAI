namespace MusicAI.Contracts.Enums
{
        public enum LoggerType
        {
            DatabaseExpLogger = 1,
            FileIOExpLogger = 2,
            OtherExpLogger = 3,
            InfoLogger = 4,
            UserLogger = 5,
            MemoryLogger = 6,
            SendingMessageLogger = 7,
            CompanyBalanceLogger = 8,
            PaymentLoggerStp1 = 9,
            PaymentLoggerStp2 = 10,
            PaymentLoggerStp3 = 11,
            PaymentLoggerStp4 = 12,
            BasketThirdPartyConnectionLogger = 13,
            BasketDBOperationFailedLogger = 14,
            BasketBaseExceptionLogger = 15,
            BasketOtherExceptionLogger = 16,
            WalletThirdPartyConnectionLogger = 18,
            WalletDBOperationFailedLogger = 19,
            WalletBaseExceptionLogger = 20,
            WalletOtherExceptionLogger = 21,
            FinancialExpLogger = 17,
        }
}

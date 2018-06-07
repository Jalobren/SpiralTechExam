using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Bank.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Bank.AppLogic
{
    public class TransactionService : ITransactionService
    {
        private IDatabase _database;
        private IAccountService _accountService;
        public TransactionService(IDatabase database, IAccountService accountService)
        {
            _database = database;
            _accountService = accountService;
        }

        public TransactionResponse Deposit(Deposit deposit, string transactionInfo = null)
        {
            var response = new TransactionResponse();
            try
            {
                using (var transaction = _database.GetTransaction(2))
                {
                    var acct = _accountService.Get(deposit.AccountId);

                    var newBalance = acct.Balance + deposit.Amount;

                    var query = @"UPDATE Accounts 
                        SET Balance = @Balance,
                            LastTransactionDate = GETUTCDATE()
                        WHERE Id = @AccountId";

                    response = ExecuteWithTransactionHistory(deposit, query, acct.AccountNumber, newBalance, transactionInfo);
                    transaction.Complete();
                }
            }
            catch (TransactionException ex)
            {
                response.ErrorCode = ErrorCodes.ERR_CONCURRENT;
            }
            catch (Exception ex)
            {
                response.ErrorCode = ErrorCodes.ERR_UNKOWN;
            }
            
            return response;
        }

        public TransactionResponse Withdraw(Withdrawal withdrawal, string transactionInfo = null)
        {
            var response = new TransactionResponse();
            try
            {
                using (var transaction = _database.GetTransaction(2))
                {
                    var acct = _accountService.Get(withdrawal.AccountId);

                    response = ValidateWithdrawalAmount(acct.Balance, withdrawal.Amount);

                    if (response.IsSuccess)
                    {
                        var newBalance = acct.Balance - withdrawal.Amount;

                        var query = @"UPDATE Accounts 
                        SET Balance = @Balance,
                            LastTransactionDate = GETUTCDATE()
                        WHERE Id = @AccountId";

                        response = ExecuteWithTransactionHistory(withdrawal, query, acct.AccountNumber, newBalance, transactionInfo);
                        transaction.Complete();
                    }
                }
            }
            catch (TransactionException ex)
            {
                response.ErrorCode = ErrorCodes.ERR_CONCURRENT;
            }
            catch (Exception ex)
            {
                response.ErrorCode = ErrorCodes.ERR_UNKOWN;
            }
            return response;
        }

        public TransactionResponse ValidateWithdrawalAmount(decimal currentBalance, decimal amount)
        {
            if (currentBalance < amount)
            {
                return new TransactionResponse { ErrorCode = ErrorCodes.ERR_INSUFFICIENT_FUNDS };
            }
            return new TransactionResponse { IsSuccess = true };
        }

        public TransactionResponse Transfer(TransferFunds transfer)
        {
            var response = new TransactionResponse();
            try
            {
                using (var transaction = _database.GetTransaction(3))
                {
                    var transferToAcct = _accountService.GetBy(transfer.TransferToAccountNumber);
                    var acct = _accountService.Get(transfer.AccountId);
                    
                    if (transferToAcct != null)
                    {
                        response = Withdraw(new Withdrawal { AccountId = transfer.AccountId, Amount = transfer.Amount, LastTransactionDate = transfer.LastTransactionDate, TransactionType = TransactionTypes.Transfer, }, $"Fund transfer to Account Name: {transferToAcct.AccountName} Account Number: {transfer.TransferToAccountNumber}");
                        if (response.IsSuccess)
                        {
                            response = Deposit(new Domain.Deposit { AccountId = transferToAcct.Id, Amount = transfer.Amount, LastTransactionDate = transfer.LastTransactionDate, TransactionType = TransactionTypes.Transfer }, $"Fund transfer from Account Name: {acct.AccountName}");
                            if (response.IsSuccess)
                            {
                                transaction.Complete();
                            }
                        }
                    }
                }
            }
            catch (TransactionException ex)
            {
                response.ErrorCode = ErrorCodes.ERR_CONCURRENT;
            }
            catch (Exception ex)
            {
                response.ErrorCode = ErrorCodes.ERR_UNKOWN;
            }

            return response;
        }

        public IEnumerable<TransactionHistory> GetHistory(int accountId)
        {
            return _database.QueryList<TransactionHistory>("SELECT * FROM TransactionHistory WHERE AccountId = @AccountId", new { AccountId = accountId });
        }

        private TransactionResponse ExecuteWithTransactionHistory(ITransaction transaction, string sqltransaction, string accountNumber, decimal newBalance, string transactionInfo = null)
        {
            var transactionTypeString = transaction.TransactionType.ToString();
            var transHistory = $"INSERT INTO TransactionHistory (AccountId, AccountNumber, TransactionType, TransactionInfo, TransactionAmount, CurrentBalance) " +
                                    $"VALUES (@AccountId, @AccountNumber, @TransactionType, @TransactionInfo, @TransactionAmount, @Balance)";

            var query = new StringBuilder();
            query.Append(sqltransaction).AppendLine();
            query.Append(transHistory);

            _database.Execute(query.ToString(), 
                                new { AccountId = transaction.AccountId,
                                        AccountNumber = accountNumber,
                                        TransactionType = transactionTypeString,
                                        TransactionInfo = transactionInfo,
                                        TransactionAmount = transaction.Amount,
                                        Balance = newBalance });

            return new TransactionResponse { IsSuccess = true };
        }
    }
}

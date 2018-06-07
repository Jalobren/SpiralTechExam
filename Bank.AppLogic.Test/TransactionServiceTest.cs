using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace Bank.AppLogic.Test
{
    [TestClass]
    public class TransactionServiceTest
    {
        private Mock<IDatabase> _mockDatabase;
        private Mock<IAccountService> _mockAccountService;

        [TestInitialize]
        public void Initialize()
        {
            _mockDatabase = new Mock<IDatabase>();
            _mockAccountService = new Mock<IAccountService>();
        }

        [TestMethod]
        public void TransactionService_Deposit_Success()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 100m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };
            
            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Deposit(new Deposit { AccountId = 1, Amount = 20, LastTransactionDate = account.LastTransactionDate, TransactionType = TransactionTypes.Deposit });

            //assert
            Assert.AreEqual(expectedResponse.IsSuccess, actual.IsSuccess);
        }

        [TestMethod]
        public void TransactionService_Deposit_Fail_Concurrency()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 100m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now.AddSeconds(10)
            };
           
            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Throws(new TransactionException());

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Deposit(new Deposit { AccountId = 1, Amount = 20, LastTransactionDate = DateTime.Now, TransactionType = TransactionTypes.Deposit });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_CONCURRENT, actual.ErrorCode);
        }

        [TestMethod]
        public void TransactionService_Withdraw_Success()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 100m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Withdraw(new Withdrawal { AccountId = 1, Amount = 20, LastTransactionDate = account.LastTransactionDate, TransactionType = TransactionTypes.Withdrawal });

            //assert
            Assert.AreEqual(expectedResponse.IsSuccess, actual.IsSuccess);
        }

        [TestMethod]
        public void TransactionService_Withdraw_Fail_Concurrency()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 100m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now.AddSeconds(10)
            };

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Throws(new TransactionException());

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Withdraw(new Withdrawal { AccountId = 1, Amount = 20, LastTransactionDate = DateTime.Now, TransactionType = TransactionTypes.Withdrawal });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_CONCURRENT, actual.ErrorCode);
        }

        [TestMethod]
        public void TransactionService_Withdraw_Fail_InsufficientFunds()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 10m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Withdraw(new Withdrawal { AccountId = 1, Amount = 20, LastTransactionDate = account.LastTransactionDate, TransactionType = TransactionTypes.Withdrawal });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_INSUFFICIENT_FUNDS, actual.ErrorCode);
        }

        [TestMethod]
        public void TransactionService_Transfer_Success()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 100m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };

            var transferAcct = new Account()
            {
                Id = 2,
                AccountName = "test account2",
                AccountNumber = "ACCT02",
                Balance = 10m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);
            _mockAccountService.Setup(x => x.GetBy(It.IsAny<string>())).Returns(transferAcct);

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Transfer(new TransferFunds { AccountId = 1, Amount = 20, LastTransactionDate = account.LastTransactionDate, TransferToAccountNumber = "ACCT02", TransactionType = TransactionTypes.Transfer });

            //assert
            Assert.AreEqual(expectedResponse.IsSuccess, actual.IsSuccess);
        }

        [TestMethod]
        public void TransactionService_Transfer_Fail_Concurrency()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 100m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now.AddSeconds(10)
            };

            var transferAcct = new Account()
            {
                Id = 2,
                AccountName = "test account2",
                AccountNumber = "ACCT02",
                Balance = 10m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now.AddSeconds(10)
            };

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);
            _mockAccountService.Setup(x => x.GetBy(It.IsAny<string>())).Throws(new TransactionException());

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Transfer(new TransferFunds { AccountId = 1, Amount = 20, LastTransactionDate = DateTime.Now, TransferToAccountNumber = "ACCT02", TransactionType = TransactionTypes.Transfer });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_CONCURRENT, actual.ErrorCode);
        }

        [TestMethod]
        public void TransactionService_Transfer_Fail_InsufficientFunds()
        {
            var expectedResponse = new TransactionResponse()
            {
                IsSuccess = true
            };
            var account = new Account()
            {
                Id = 1,
                AccountName = "test account",
                AccountNumber = "ACCT01",
                Balance = 10m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };
            var transferAcct = new Account()
            {
                Id = 2,
                AccountName = "test account2",
                AccountNumber = "ACCT02",
                Balance = 10m,
                Password = "testPassword",
                LastTransactionDate = DateTime.Now
            };

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>())).Returns(new TransactionResponse { IsSuccess = true });
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);
            _mockAccountService.Setup(x => x.GetBy(It.IsAny<string>())).Returns(transferAcct);

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Transfer(new TransferFunds { AccountId = 1, Amount = 20, LastTransactionDate = account.LastTransactionDate, TransferToAccountNumber = "ACCT02", TransactionType = TransactionTypes.Transfer });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_INSUFFICIENT_FUNDS, actual.ErrorCode);
        }
    }
}

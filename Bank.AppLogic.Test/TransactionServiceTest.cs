using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

            _mockDatabase.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>())).Returns(1);
            _mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(account);

            //act
            var service = new TransactionService(_mockDatabase.Object, _mockAccountService.Object);
            var actual = service.Deposit(new Deposit { AccountId = 1, Amount = 20, LastTransactionDate = account.LastTransactionDate, TransactionType = TransactionTypes.Deposit });

            //assert
            Assert.AreEqual(expectedResponse.IsSuccess, actual.IsSuccess);
        }
    }
}

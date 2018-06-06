using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Bank.AppLogic.Test
{
    [TestClass]
    public class AccountServiceTest
    {
        private Mock<IDatabase> _mockDatabase;

        [TestInitialize]
        public void Initialize()
        {
            _mockDatabase = new Mock<IDatabase>();
        }
        [TestMethod]
        public void AccountService_GetById_Success()
        {
            //arrange
            var expected = new Account {
                Id = 1,
                AccountName = "test account",
                Balance = 100m,
                AccountNumber = "ACCT01",
                Password = "Pass1",
                LastTransactionDate = DateTime.Now
            };
            _mockDatabase.Setup(x => x.Query<Account>(It.IsAny<string>(), It.IsAny<object>())).Returns(expected);
            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.GetBy("ACCT01");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AccountService_GetById_Null()
        {
            //arrange
            Account expected = null;
            _mockDatabase.Setup(x => x.Query<Account>(It.IsAny<string>(), It.IsAny<object>())).Returns(expected);
            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.GetBy("ACCT02");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AccountService_GetBy_AccountNumber_Password_Success()
        {
            //arrange
            var expected = new Account
            {
                Id = 1,
                AccountName = "John smith",
                Balance = 100m,
                AccountNumber = "ACCT04",
                Password = "Pass1",
                LastTransactionDate = DateTime.Now
            };
            _mockDatabase.Setup(x => x.Query<Account>(It.IsAny<string>(), It.IsAny<object>())).Returns(expected);
            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.GetBy("ACCT04", "Pass1");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AccountService_GetBy_AccountNumber_Password_Null()
        {
            //arrange
            Account expected = null;
            _mockDatabase.Setup(x => x.Query<Account>(It.IsAny<string>(), It.IsAny<object>())).Returns(expected);
            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.GetBy("ACCT04", "Pass234");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AccountService_CreateAccount_Success()
        {
            //arrange
            Account expected = null;
            _mockDatabase.Setup(x => x.Query<int>(It.IsAny<string>(), It.IsAny<object>())).Returns(1);
            _mockDatabase.Setup(x => x.Query<Account>(It.IsAny<string>(), It.IsAny<object>())).Returns(expected);

            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.Create(new Account { AccountNumber = "ACCT05", AccountName = "Account 5", Password = "password1", Balance = 0 });

            //assert
            Assert.AreEqual(true, actual.IsSuccess);
        }

        [TestMethod]
        public void AccountService_CreateAccount_Fail_Duplicate_AccountNumber()
        {
            //arrange
            var existingAcct = new Account
            {
                Id = 1,
                AccountName = "test account",
                Balance = 100m,
                AccountNumber = "ACCT01",
                Password = "Pass1",
                LastTransactionDate = DateTime.Now
            };
            _mockDatabase.Setup(x => x.Query<int>(It.IsAny<string>(), It.IsAny<object>())).Returns(1);
            _mockDatabase.Setup(x => x.Query<Account>("SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber", It.IsAny<object>())).Returns(existingAcct);

            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.Create(new Account { AccountNumber = "ACCT01", AccountName = "abc", Password = "password1", Balance = 0 });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_ACCOUNT_DUPLICATE_ACCT_NUMBER, actual.ErrorCode);
        }

        [TestMethod]
        public void AccountService_CreateAccount_Fail_Duplicate_AccountName()
        {
            //arrange
            var existingAcct = new Account
            {
                Id = 1,
                AccountName = "test account",
                Balance = 100m,
                AccountNumber = "ACCT01",
                Password = "Pass1",
                LastTransactionDate = DateTime.Now
            };
            _mockDatabase.Setup(x => x.Query<int>(It.IsAny<string>(), It.IsAny<object>())).Returns(1);
            _mockDatabase.Setup(x => x.Query<Account>("SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber", It.IsAny<object>())).Returns((Account)null);
            _mockDatabase.Setup(x => x.Query<Account>("SELECT * FROM Accounts WHERE AccountName = @AccountName", It.IsAny<object>())).Returns(existingAcct);

            //act
            var service = new AccountService(_mockDatabase.Object);
            var actual = service.Create(new Account { AccountNumber = "ACCT06", AccountName = "test account", Password = "password1", Balance = 0 });

            //assert
            Assert.AreEqual(ErrorCodes.ERR_ACCOUNT_DUPLICATE_ACCT_NAME, actual.ErrorCode);
        }
    }
}

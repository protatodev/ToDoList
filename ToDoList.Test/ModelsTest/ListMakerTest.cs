using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System;
using System.Collections.Generic;

namespace ToDoList.Tests
{

    [TestClass]
    public class ListMakerTest : IDisposable
    {
        public void Dispose()
        {
            ListMaker.DeleteAll();
           // Category.DeleteAll();
        }

        public ListMakerTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {
            //Arrange
            //Act
            int result = ListMaker.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
        {
            // Arrange, Act
            ListMaker firstItem = new ListMaker("Mow the lawn", 1);
            ListMaker secondItem = new ListMaker("Mow the lawn", 1);

            // Assert
            Assert.AreEqual(firstItem, secondItem);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ItemList()
        {
            //Arrange
            ListMaker testItem = new ListMaker("Mow the lawn", 1);

            //Act
            testItem.Save();
            List<ListMaker> result = ListMaker.GetAll();
            List<ListMaker> testList = new List<ListMaker> { testItem };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            //Arrange
            ListMaker testItem = new ListMaker("Mow the lawn", 1);

            //Act
            testItem.Save();
            ListMaker savedItem = ListMaker.GetAll()[0];

            int result = savedItem.GetId();
            int testId = testItem.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
            //Arrange
            ListMaker testItem = new ListMaker("Mow the lawn", 1);
            testItem.Save();

            //Act
            ListMaker foundItem = ListMaker.Find(testItem.GetId());

            //Assert
            Assert.AreEqual(testItem, foundItem);
        }

        [TestMethod]
        public void Edit_UpdatesItemInDatabase_String()
        {
            //Arrange
            string firstDescription = "Walk the Dog";
            ListMaker testItem = new ListMaker(firstDescription, 1);
            testItem.Save();
            string secondDescription = "Mow the lawn";

            //Act

            testItem.Edit(secondDescription);
            string result = ListMaker.Find(testItem.GetId()).GetDescription();

            //Assert
            Assert.AreEqual(secondDescription, result);
        }
    }
}
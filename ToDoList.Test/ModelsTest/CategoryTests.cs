using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;

namespace ToDoList.Tests
{
    [TestClass]
    public class CategoryTests : IDisposable
    {
        public void Dispose()
        {
            ListMaker.DeleteAll();
            Category.DeleteAll();
        }

        public CategoryTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

        [TestMethod]
        public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
        {
            //Arrange
            ListMaker testItem = new ListMaker("Mow the lawn");
            testItem.Save();

            string testName = "Home stuff";
            Category testCategory = new Category(testName);
            testCategory.Save();

            //Act
            testCategory.AddItem(testItem);
            testCategory.Delete();

            List<Category> resultItemCategories = testItem.GetCategories();
            List<Category> testItemCategories = new List<Category> { };

            //Assert
            CollectionAssert.AreEqual(testItemCategories, resultItemCategories);
        }

        [TestMethod]
        public void Test_AddItem_AddsItemToCategory()
        {
            //Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            ListMaker testItem = new ListMaker("Mow the lawn");
            testItem.Save();

            ListMaker testItem2 = new ListMaker("Water the garden");
            testItem2.Save();

            //Act
            testCategory.AddItem(testItem);
            testCategory.AddItem(testItem2);

            List<ListMaker> result = testCategory.GetItems();
            List<ListMaker> testList = new List<ListMaker> { testItem, testItem2 };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetItems_ReturnsAllCategoryItems_ItemList()
        {
            //Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            ListMaker testItem1 = new ListMaker("Mow the lawn");
            testItem1.Save();

            ListMaker testItem2 = new ListMaker("Buy plane ticket");
            testItem2.Save();

            //Act
            testCategory.AddItem(testItem1);
            List<ListMaker> savedItems = testCategory.GetItems();
            List<ListMaker> testList = new List<ListMaker> { testItem1 };

            //Assert
            CollectionAssert.AreEqual(testList, savedItems);
        }

        /*
        [TestMethod]
        public void GetItems_RetrievesAllItemsWithCategory_ItemList()
        {
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            ListMaker firstItem = new ListMaker("Mow the lawn");
            firstItem.Save();
            ListMaker secondItem = new ListMaker("Do the dishes");
            secondItem.Save();


            List<ListMaker> testItemList = new List<ListMaker> { firstItem, secondItem };
            List<ListMaker> resultItemList = testCategory.GetItems();

            CollectionAssert.AreEqual(testItemList, resultItemList);
        }
        */

        [TestMethod]
        public void GetAll_CategoriesEmptyAtFirst_0()
        {
            //Arrange, Act
            int result = Category.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueForSameName_Category()
        {
            //Arrange, Act
            Category firstCategory = new Category("Household chores");
            Category secondCategory = new Category("Household chores");

            //Assert
            Assert.AreEqual(firstCategory, secondCategory);
        }

        [TestMethod]
        public void Save_SavesCategoryToDatabase_CategoryList()
        {
            //Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            //Act
            List<Category> result = Category.GetAll();
            List<Category> testList = new List<Category> { testCategory };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }


        [TestMethod]
        public void Save_DatabaseAssignsIdToCategory_Id()
        {
            //Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            //Act
            Category savedCategory = Category.GetAll()[0];

            int result = savedCategory.GetId();
            int testId = testCategory.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }


        [TestMethod]
        public void Find_FindsCategoryInDatabase_Category()
        {
            //Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            //Act
            Category foundCategory = Category.Find(testCategory.GetId());

            //Assert
            Assert.AreEqual(testCategory, foundCategory);
        }
    }
}
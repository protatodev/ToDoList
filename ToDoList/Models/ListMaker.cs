using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDoList;
using System;

namespace ToDoList.Models
{
    public class ListMaker
    {
        private int id = 0;
        private string description = "";

        public ListMaker(string Description, int Id =0)
        {
            id = Id;
            description = Description;
        }

        public override int GetHashCode()
        {
            return this.GetDescription().GetHashCode();
        }

        public string GetDescription()
        {
            return description;
        }

        public int GetId()
        {
            return id;
        }

        public void AddCategory(Category newCategory)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CategoryId";
            category_id.Value = newCategory.GetId();
            cmd.Parameters.Add(category_id);

            MySqlParameter item_id = new MySqlParameter();
            item_id.ParameterName = "@ItemId";
            item_id.Value = id;
            cmd.Parameters.Add(item_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Category> GetCategories()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT categories.* FROM items
                                JOIN categories_items ON (items.id = categories_items.item_id)
                                JOIN categories ON (categories_items.category_id = categories.id)
                                WHERE items.id = @itemId;";

            MySqlParameter itemIdParameter = new MySqlParameter();
            itemIdParameter.ParameterName = "@itemId";
            itemIdParameter.Value = id;
            cmd.Parameters.Add(itemIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Category> categories = new List<Category> { };

            while (rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);
                Category newCategory = new Category(categoryName, categoryId);
                categories.Add(newCategory);
            }
           
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return categories;
        }

        public void Edit(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            this.description = newDescription;

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items WHERE id = @ItemId; DELETE FROM categories_items WHERE item_id = @ItemId;";

            MySqlParameter itemIdParameter = new MySqlParameter();
            itemIdParameter.ParameterName = "@ItemId";
            itemIdParameter.Value = this.GetId();
            cmd.Parameters.Add(itemIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static List<ListMaker> GetAll()
        {
            List<ListMaker> allItems = new List<ListMaker> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                ListMaker newItem = new ListMaker(itemDescription, itemId);
                allItems.Add(newItem);
            }

            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }

            return allItems;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherItem)
        {
            if (!(otherItem is ListMaker))
            {
                return false;
            }
            else
            {
                ListMaker newItem = (ListMaker) otherItem;
                bool idEquality = (this.GetId() == newItem.GetId());
                bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());

                return (idEquality && descriptionEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (description) VALUES (@description);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this.description;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            id = (int) cmd.LastInsertedId;

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public static ListMaker Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int itemId = 0;
            string itemDescription = "";

            while(rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
            }

            ListMaker foundItem = new ListMaker(itemDescription, itemId);

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }

            return foundItem;
        }
    }
}

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
        private int categoryId;

        public ListMaker(string Description, int categoryId, int Id =0)
        {
            id = Id;
            this.categoryId = categoryId;
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

        public int GetCategoryId()
        {
            return categoryId;
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
            cmd.CommandText = @"DELETE FROM items WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
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
                int itemCategoryId = rdr.GetInt32(2);
                ListMaker newItem = new ListMaker(itemDescription, itemCategoryId, itemId);
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
                bool categoryEquality = this.GetCategoryId() == newItem.GetCategoryId();

                return (idEquality && descriptionEquality && categoryEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (description, category_id) VALUES (@description, @category_id);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this.description;
            cmd.Parameters.Add(description);

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@category_id";
            categoryId.Value = this.categoryId;
            cmd.Parameters.Add(categoryId);


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
            int itemCategoryId = 0;

            while(rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
                itemCategoryId = rdr.GetInt32(2);
            }

            ListMaker foundItem = new ListMaker(itemDescription, itemCategoryId, itemId);

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }

            return foundItem;
        }
    }
}

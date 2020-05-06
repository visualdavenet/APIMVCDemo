﻿using APIMVCDemo.Data.DataAccess;
using APIMVCDemo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIMVCDemo.Data.BusinessLogic
{
    public static class CustomerProcessor
    {
        public static int CreateCustomer(string firstName, string lastName, string occupation,
            string city, string state, string email, string imageURL)
        {
            CustomerDataModel data = new CustomerDataModel
            {
                FirstName = firstName,
                LastName = lastName,
                Occupation = occupation,
                City = city,
                State = state,
                Email = email,
                ImageURL = imageURL
            };

            string sql = @"insert into dbo.Customers (FirstName, LastName, Occupation, City, State, Email, ImageURL)
                            values (@FirstName, @LastName, @Occupation, @City, @State, @Email, @ImageURL);";

            return SQLDataAccess.SaveData(sql, data);

        }

        public static List<CustomerDataModel> LoadCustomers()
        {
            string sql = @"select FirstName, LastName, Occupation, City, State, Email, ImageURL 
                            from dbo.Customers";

            return SQLDataAccess.LoadData<CustomerDataModel>(sql).ToList();
        }
    }
}
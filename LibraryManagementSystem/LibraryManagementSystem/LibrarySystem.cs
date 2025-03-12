using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
namespace LibraryManagementSystem
{
    class LibrarySystem
    {
        private List<BookDTO> books=new List<BookDTO>();
        private List<MemberDTO> members=new List<MemberDTO>();
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = LibrarySystem; Integrated Security = True";
        SqlConnection connection;
        public LibrarySystem()
        {
            connection = new SqlConnection(connectionString);
        }
        //Adds new member in the member table 
        public void AddMember(MemberDTO m)
        {
            connection.Open();
            string query = $"INSERT INTO Member (Name,MCNIC) VALUES ('{m.Name}','{m.CNIC}' )";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Member added successfully!");
            members.Add(m);
            connection.Close();
        }
        //updates the member details by searching the cnic in the member table
        public void UpdateMember(string cnic, string newName)
        {
            connection.Open();
            string query = $"UPDATE Member SET Name = '{newName}' WHERE MCNIC = '{cnic}'";
            SqlCommand command = new SqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
                Console.WriteLine("Member updated successfully!");
            else
                Console.WriteLine("Member not found!");
            connection.Close();
        }
        //deletes member by searching the cnic in the member table
        public void DeleteMember(string cnic)
        {
            connection.Open();
            string query = $"DELETE FROM Member WHERE MCNIC = '{cnic}'";
            SqlCommand command = new SqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
                Console.WriteLine("Member deleted successfully!");
            else
                Console.WriteLine("Member not found!");
            connection.Close();
        }
        //displays all of the members in the member table
        public void DisplayMembers()
        {
            connection.Open();
            string query = "SELECT * FROM Member";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            bool hasRecords = false;
            while (reader.Read())
            {
                hasRecords = true;
                Console.WriteLine($"Name: {reader["Name"]}, CNIC: {reader["MCNIC"]}");
            }
            if (!hasRecords)
            {
                Console.WriteLine("No members found in the database.");
            }
            reader.Close();
            connection.Close();
        }
        //adds book in the book table
        public void AddBook(BookDTO b)
        {
            connection.Open();
            int v = 0;
            if (b.IsIssued) {
                v = 1;
            }
            string query = $"INSERT INTO Book (BookID,Title, Author, IsIssued,BorrowCount) VALUES ({b.BookID},'{b.Title}', '{b.Author}', {v},{0})";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Book added successfully!");
            books.Add(b);
            connection.Close();
            
        }
        /*updates the book title and author name by searching the book id in book table.
         * As The bookID is randomly generated whenever we will do any updation or deletion first we will display all the book details
         * to know which bookID we want to delete as book ID is primary key in database*/
        public void UpdateBook(int bookID, string newTitle, string newAuthor)
        {
            connection.Open();
            string query = $"UPDATE Book SET Title = '{newTitle}', Author = '{newAuthor}' WHERE BookID = {bookID}";
            SqlCommand command = new SqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
                Console.WriteLine("Book updated successfully!");
            else
                Console.WriteLine("Book not found!");
            connection.Close();
        }
        //deletes book by searching bookID in table. 
        public void DeleteBook(int bookID)
        {
            try
            {
                connection.Open();
                string checkQuery = $"SELECT IsIssued FROM Book WHERE BookID = {bookID}";
                SqlCommand command = new SqlCommand(checkQuery, connection);
                object result = command.ExecuteScalar();
                if (result != null && (bool)result)
                {
                    Console.WriteLine("Cannot delete the book. It is currently issued.");
                    return;
                }
                string deleteBorrowedQuery = $"DELETE FROM BorrowedBooks WHERE BookID = {bookID}";
                SqlCommand deleteBorrowedCmd = new SqlCommand(deleteBorrowedQuery, connection);
                deleteBorrowedCmd.ExecuteNonQuery();
                string deleteBookQuery = $"DELETE FROM Book WHERE BookID = {bookID}";
                SqlCommand deleteBookCmd = new SqlCommand(deleteBookQuery, connection);
                int rowsAffected = deleteBookCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Book deleted successfully!");
                else
                    Console.WriteLine("Book not found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        //displays all the books
        public void DisplayBooks()
        {
            connection.Open();
            string query = "SELECT * FROM Book";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            bool hasRecords = false;
            while (reader.Read()) 
            {
                hasRecords = true;
                bool flag = (bool)reader["IsIssued"];
                string status;
                if (flag)
                {
                    status = "Issued";
                }
                else
                {
                    status = "Available";
                }
                    Console.WriteLine($"BookID: {reader["BookID"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Status: {status}");
            }
            if (!hasRecords)
            {
                Console.WriteLine("No books found in the database.");
            }
            reader.Close();
            connection.Close();
        }
        //displays available books. used this for easy book issuance.
        public bool DisplayAvailableBooks()
        {
            connection.Open();
            string query = "SELECT * FROM Book WHERE IsIssued = 0";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            bool hasRecords = false;
            Console.WriteLine("Available Books: ");
            while (reader.Read())
            {
                hasRecords = true;
                Console.WriteLine($"BookID: {reader["BookID"]}, Title: {reader["Title"]}, Author: {reader["Author"]}");
            }
            if (!hasRecords)
            {
                Console.WriteLine("No Available books found in the database.");
            }
            reader.Close();
            connection.Close();
            return hasRecords;
        }
        //Issues book to a member and place the bookID and member cnic in the borrowedBook table
        public void IssueBook(int bookID, string cnic)
        {
            connection.Open();
            string checkBookQuery = $"SELECT IsIssued FROM Book WHERE BookID = {bookID}";
            SqlCommand checkBookCommand = new SqlCommand(checkBookQuery, connection);
            bool isIssued = (bool)checkBookCommand.ExecuteScalar();

            if (isIssued)
            {
                Console.WriteLine("Book is already issued!");
                connection.Close();
                return;
            }
            string issueBookQuery = $"UPDATE Book SET IsIssued = 1 WHERE BookID = {bookID}";
            SqlCommand issueBookCommand = new SqlCommand(issueBookQuery, connection);
            issueBookCommand.ExecuteNonQuery();
            string addBorrowQuery = $"INSERT INTO BorrowedBooks (MCNIC, BookID) VALUES ('{cnic}', {bookID})";
            SqlCommand addBorrowCommand = new SqlCommand(addBorrowQuery, connection);
            addBorrowCommand.ExecuteNonQuery();
            string updateBorrowCountQuery = $"UPDATE Book SET BorrowCount = BorrowCount + 1 WHERE BookID = {bookID}";
            SqlCommand updateBorrowCountCommand = new SqlCommand(updateBorrowCountQuery, connection);
            updateBorrowCountCommand.ExecuteNonQuery();

            Console.WriteLine("Book issued successfully!");

            connection.Close();
        }
        //this displays all the bookIDs of a single member. this helps to return the book easily.
        public bool DisplayMemberBorrowedBookIDs(string cnic)
        {
            connection.Open();
            string query = $"SELECT * FROM BorrowedBooks WHERE MCNIC='{cnic}' ";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            bool hasRecords = false;
            Console.WriteLine("Books Borrowed by this Member: ");
            while (reader.Read())
            {
                hasRecords = true;
                Console.WriteLine($"BookID: {reader["BookID"]}");
            }
            if (!hasRecords)
            {
                Console.WriteLine("No record found in the database.");
            }
            reader.Close();
            connection.Close();
            return hasRecords;
        }
        //this updates the borrowedBook table and removes the entry where cnic and bookID matches and updates the book IsIssued as available.
        public void ReturnBook(int bookID, string cnic)
        {
            connection.Open();
            string checkBorrowQuery = $"SELECT COUNT(*) FROM BorrowedBooks WHERE MCNIC = '{cnic}' AND BookID = {bookID}";
            SqlCommand checkBorrowCommand = new SqlCommand(checkBorrowQuery, connection);
            int count = Convert.ToInt32(checkBorrowCommand.ExecuteScalar());
            if (count == 0)
            {
                Console.WriteLine("Book was not issued to this member!");
                return;
            }
            string returnBookQuery = $"UPDATE Book SET IsIssued = 0 WHERE BookID = {bookID}";
            SqlCommand returnBookCommand = new SqlCommand(returnBookQuery, connection);
            returnBookCommand.ExecuteNonQuery();
            string removeBorrowQuery = $"DELETE FROM BorrowedBooks WHERE MCNIC = '{cnic}' AND BookID = {bookID}";
            SqlCommand removeBorrowCommand = new SqlCommand(removeBorrowQuery, connection);
            removeBorrowCommand.ExecuteNonQuery();
            Console.WriteLine("Book returned successfully!");
            connection.Close();
        }
        //this uses the borrowCount attribute in the book table to get the most borrowed book
        public void DeclareMostBorrowedBook()
        {
            connection.Open();
            string maxBorrowCountQuery = "SELECT MAX(BorrowCount) FROM Book";
            SqlCommand maxBorrowCountCommand = new SqlCommand(maxBorrowCountQuery, connection);
            object result = maxBorrowCountCommand.ExecuteScalar();
            if (result == DBNull.Value || result == null || Convert.ToInt32(result) == 0)
            {
                Console.WriteLine("No books have been borrowed yet.");
                connection.Close();
                return;
            }
            int maxBorrowCount = Convert.ToInt32(result);
            string query = @"
                             SELECT BookID, Title, Author, BorrowCount
                             FROM Book
                             WHERE BorrowCount = @maxBorrowCount";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@maxBorrowCount", maxBorrowCount);
            SqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("Most borrowed book(s):");
            while (reader.Read())
            {
                Console.WriteLine($"BookID: {reader["BookID"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Borrowed {reader["BorrowCount"]} times");
            }
            connection.Close();
        }

    }

}

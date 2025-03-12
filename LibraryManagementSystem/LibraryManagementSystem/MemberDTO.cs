using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    class MemberDTO
    {
        private string name;
        private string cnic;
        private List<int> borrowedBooks;
        private static int membersCount=0;
        public MemberDTO()
        {
            name = null;
            cnic = null;
        }
        public MemberDTO(string name, string cnic)
        {
            this.name = name;
            this.cnic = cnic;
            this.borrowedBooks = new List<int>();
        }
        public string CNIC
        {
            get { return cnic; }
        }
        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }
        public bool HasBorrowed(int bookID)
        {
            return borrowedBooks.Contains(bookID);
        }
        public void BorrowBook(int bookID)
        {
            borrowedBooks.Add(bookID);
        }
        public void ReturnBook(int bookID)
        {
            borrowedBooks.Remove(bookID);
        }
        public override string ToString()
        {
            return $"Name: {name}, CNIC: {cnic}, Borrowed Books: {string.Join(", ", borrowedBooks)}";
        }
    }
}

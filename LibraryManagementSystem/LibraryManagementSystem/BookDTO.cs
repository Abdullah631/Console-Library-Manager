using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    class BookDTO
    {
        private int bookID;
        private string title;
        private string author;
        private bool isIssued;
        private int GenBookID()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
        public BookDTO(string title, string author)
        {
            bookID = GenBookID();
            this.title = title;
            this.author = author;
            this.isIssued = false;
        }
        public int BookID
        {
            get { return bookID; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Author
        {
            get { return author; }
            set { this.author = value; }
        }
        public bool IsIssued
        {
            get { return isIssued; }
        }
        public void IssueBook()
        {
            this.isIssued = true;
        }
        public void ReturnBook()
        {
            this.isIssued = false;
        }
        public override string ToString()
        {
            string status;
            if (this.isIssued)
            {
                status = "Issued";
            }
            else
            {
                status = "Available";
            }
            return $"BookID: {bookID}, Title: {title}, Author: {author}, Status: {status}";
        }
}
}

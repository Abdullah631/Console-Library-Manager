using LibraryManagementSystem;
internal class Program
{
    private static void Main(string[] args)
    {
        LibrarySystem library = new LibrarySystem();
        bool flag = true;
        while (flag)
        {
            Console.WriteLine("\n\nLibrary Management System");
            Console.WriteLine("1. AddMember");
            Console.WriteLine("2. UpdateMember");
            Console.WriteLine("3. DeleteMember");
            Console.WriteLine("4. DisplayMembers");
            Console.WriteLine("5. AddBook");
            Console.WriteLine("6. UpdateBook");
            Console.WriteLine("7. DeleteBook");
            Console.WriteLine("8. DisplayBooks");
            Console.WriteLine("9. IssueBook");
            Console.WriteLine("10. ReturnBook");
            Console.WriteLine("11. DeclareMostBorrowedBook");
            Console.WriteLine("12. Exit\n\n");
            Console.WriteLine("Enter your choice from 1 to 12: ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter member name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter member CNIC: ");
                    string cnic = Console.ReadLine();
                    MemberDTO m = new MemberDTO(name, cnic);
                    library.AddMember(m);
                    break;
                case 2:
                    Console.WriteLine("Enter member CNIC: ");
                    cnic = Console.ReadLine();
                    Console.WriteLine("Enter new name: ");
                    name = Console.ReadLine();
                    library.UpdateMember(cnic, name);
                    break;
                case 3:
                    Console.WriteLine("Enter member CNIC: ");
                    cnic = Console.ReadLine();
                    library.DeleteMember(cnic);
                    break;
                case 4:
                    library.DisplayMembers();
                    break;
                case 5:
                    Console.WriteLine("Enter book title: ");
                    string title = Console.ReadLine();
                    Console.WriteLine("Enter book author: ");
                    string author = Console.ReadLine();
                    BookDTO b = new BookDTO(title, author);
                    library.AddBook(b);
                    break;
                case 6:
                    library.DisplayBooks();
                    Console.WriteLine("Enter book ID: ");
                    int bookID = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter new title: ");
                    title = Console.ReadLine();
                    Console.WriteLine("Enter new author: ");
                    author = Console.ReadLine();
                    library.UpdateBook(bookID, title, author);
                    break;
                case 7:
                    library.DisplayBooks();
                    Console.WriteLine("Enter book ID: ");
                    bookID = int.Parse(Console.ReadLine());
                    library.DeleteBook(bookID);
                    break;
                case 8:
                    library.DisplayBooks();
                    break;
                case 9:
                    if (library.DisplayAvailableBooks())
                    {

                        Console.WriteLine("Enter book ID: ");
                        bookID = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter member CNIC: ");
                        cnic = Console.ReadLine();
                        library.IssueBook(bookID, cnic);
                        break;
                    }
                    else
                    {
                        break;
                    }
                case 10:
                    Console.WriteLine("Enter member CNIC: ");
                    cnic = Console.ReadLine();
                    library.DisplayMemberBorrowedBookIDs(cnic);
                    Console.WriteLine("Enter book ID: ");
                    bookID = int.Parse(Console.ReadLine());
                    library.ReturnBook(bookID, cnic);
                    break;
                case 11:
                    library.DeclareMostBorrowedBook();
                    break;
                case 12:
                    flag = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
}
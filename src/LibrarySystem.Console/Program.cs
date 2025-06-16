using LibrarySystem.Models;
using LibrarySystem.Services;

namespace LibrarySystem.Console;

class Program
{
    private static readonly Library _library = new();
    private static readonly FineCalculator _fineCalculator = new(0.50m, 30.00m);

    static void Main(string[] args)
    {
        System.Console.WriteLine("Welcome to Library Management System");
        
        while (true)
        {
            ShowMainMenu();
            var choice = System.Console.ReadLine();

            try
            {
                switch (choice?.ToLower())
                {
                    case "1":
                        HandleMemberManagement();
                        break;
                    case "2":
                        HandleBookManagement();
                        break;
                    case "3":
                        HandleBorrowingOperations();
                        break;
                    case "4":
                        System.Console.WriteLine("Thank you for using the Library Management System!");
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
                System.Console.WriteLine("Press any key to continue...");
                System.Console.ReadKey();
            }
        }
    }

    private static void ShowMainMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("\nMain Menu:");
        System.Console.WriteLine("1. Member Management");
        System.Console.WriteLine("2. Book Management");
        System.Console.WriteLine("3. Borrowing Operations");
        System.Console.WriteLine("4. Exit");
        System.Console.Write("\nEnter your choice: ");
    }

    private static void HandleMemberManagement()
    {
        while (true)
        {
            System.Console.Clear();
            System.Console.WriteLine("\nMember Management:");
            System.Console.WriteLine("1. Register New Member");
            System.Console.WriteLine("2. View Member Details");
            System.Console.WriteLine("3. Back to Main Menu");
            System.Console.Write("\nEnter your choice: ");

            var choice = System.Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RegisterNewMember();
                    break;
                case "2":
                    ViewMemberDetails();
                    break;
                case "3":
                    return;
                default:
                    System.Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void HandleBookManagement()
    {
        while (true)
        {
            System.Console.Clear();
            System.Console.WriteLine("\nBook Management:");
            System.Console.WriteLine("1. Add New Book");
            System.Console.WriteLine("2. Search Books");
            System.Console.WriteLine("3. View Available Books");
            System.Console.WriteLine("4. Back to Main Menu");
            System.Console.Write("\nEnter your choice: ");

            var choice = System.Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNewBook();
                    break;
                case "2":
                    SearchBooks();
                    break;
                case "3":
                    ViewAvailableBooks();
                    break;
                case "4":
                    return;
                default:
                    System.Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void HandleBorrowingOperations()
    {
        while (true)
        {
            System.Console.Clear();
            System.Console.WriteLine("\nBorrowing Operations:");
            System.Console.WriteLine("1. Borrow Book");
            System.Console.WriteLine("2. Return Book");
            System.Console.WriteLine("3. View Overdue Books");
            System.Console.WriteLine("4. Check Fines");
            System.Console.WriteLine("5. Back to Main Menu");
            System.Console.Write("\nEnter your choice: ");

            var choice = System.Console.ReadLine();
            switch (choice)
            {
                case "1":
                    BorrowBook();
                    break;
                case "2":
                    ReturnBook();
                    break;
                case "3":
                    ViewOverdueBooks();
                    break;
                case "4":
                    CheckFines();
                    break;
                case "5":
                    return;
                default:
                    System.Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void RegisterNewMember()
    {
        System.Console.Write("Enter member name: ");
        var name = System.Console.ReadLine() ?? "";
        System.Console.Write("Enter member ID (format: AB1234): ");
        var id = System.Console.ReadLine() ?? "";

        var member = new Member(name, id);
        var result = _library.RegisterMember(member);

        System.Console.WriteLine(result.Message);
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey();
    }

    private static void ViewMemberDetails()
    {
        System.Console.Write("Enter member ID: ");
        var id = System.Console.ReadLine() ?? "";

        var member = _library.GetMemberById(id);
        if (member == null)
        {
            System.Console.WriteLine("Member not found.");
        }
        else
        {
            System.Console.WriteLine($"\nMember Details:");
            System.Console.WriteLine($"Name: {member.Name}");
            System.Console.WriteLine($"ID: {member.Id}");
            System.Console.WriteLine($"Borrowed Books: {member.BorrowedBooks.Count}");
        }

        System.Console.WriteLine("\nPress any key to continue...");
        System.Console.ReadKey();
    }

    private static void AddNewBook()
    {
        System.Console.Write("Enter book title: ");
        var title = System.Console.ReadLine() ?? "";
        System.Console.Write("Enter author: ");
        var author = System.Console.ReadLine() ?? "";
        System.Console.Write("Enter ISBN: ");
        var isbn = System.Console.ReadLine() ?? "";

        var book = new Book(title, author, isbn);
        var result = _library.AddBook(book);

        System.Console.WriteLine(result.Message);
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey();
    }

    private static void SearchBooks()
    {
        System.Console.Write("Enter search query: ");
        var query = System.Console.ReadLine() ?? "";

        var books = _library.SearchBooks(query);
        if (!books.Any())
        {
            System.Console.WriteLine("No books found.");
        }
        else
        {
            System.Console.WriteLine("\nFound Books:");
            foreach (var book in books)
            {
                System.Console.WriteLine($"Title: {book.Title}");
                System.Console.WriteLine($"Author: {book.Author}");
                System.Console.WriteLine($"ISBN: {book.ISBN}");
                System.Console.WriteLine($"Status: {book.Status}");
                System.Console.WriteLine();
            }
        }

        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey();
    }

    private static void ViewAvailableBooks()
    {
        var books = _library.GetAllAvailableBooks();
        if (!books.Any())
        {
            System.Console.WriteLine("No books available.");
        }
        else
        {
            System.Console.WriteLine("\nAvailable Books:");
            foreach (var book in books)
            {
                System.Console.WriteLine($"Title: {book.Title}");
                System.Console.WriteLine($"Author: {book.Author}");
                System.Console.WriteLine($"ISBN: {book.ISBN}");
                System.Console.WriteLine();
            }
        }

        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey();
    }

    private static void BorrowBook()
    {
        System.Console.Write("Enter member ID: ");
        var memberId = System.Console.ReadLine() ?? "";
        System.Console.Write("Enter book ISBN: ");
        var isbn = System.Console.ReadLine() ?? "";

        var result = _library.BorrowBook(memberId, isbn);
        System.Console.WriteLine(result.Message);
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey();
    }

    private static void ReturnBook()
    {
        System.Console.Write("Enter member ID: ");
        var memberId = System.Console.ReadLine() ?? "";
        System.Console.Write("Enter book ISBN: ");
        var isbn = System.Console.ReadLine() ?? "";

        var result = _library.ReturnBook(memberId, isbn);
        System.Console.WriteLine(result.Message);
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey();
    }

    private static void ViewOverdueBooks()
    {
        var membersWithOverdueBooks = _library.GetMembersWithOverdueBooks(DateTime.Now);
        if (!membersWithOverdueBooks.Any())
        {
            System.Console.WriteLine("No overdue books.");
        }
        else
        {
            System.Console.WriteLine("\nMembers with Overdue Books:");
            foreach (var member in membersWithOverdueBooks)
            {
                System.Console.WriteLine($"\nMember: {member.Name} (ID: {member.Id})");
                var overdueBooks = member.GetOverdueBooks(DateTime.Now);
                foreach (var book in overdueBooks)
                {
                    System.Console.WriteLine($"- {book.Title} (Due: {book.DueDate:d})");
                }
            }
        }

        System.Console.WriteLine("\nPress any key to continue...");
        System.Console.ReadKey();
    }

    private static void CheckFines()
    {
        System.Console.Write("Enter member ID: ");
        var memberId = System.Console.ReadLine() ?? "";

        var member = _library.GetMemberById(memberId);
        if (member == null)
        {
            System.Console.WriteLine("Member not found.");
        }
        else
        {
            var totalFine = _fineCalculator.CalculateTotalFines(member, DateTime.Now);
            System.Console.WriteLine($"\nTotal fines for {member.Name}: ${totalFine:F2}");

            if (totalFine > 0)
            {
                System.Console.WriteLine("\nOverdue Books:");
                var overdueBooks = member.GetOverdueBooks(DateTime.Now);
                foreach (var book in overdueBooks)
                {
                    var (daysOverdue, fine) = _fineCalculator.GetFineDetails(book, DateTime.Now);
                    System.Console.WriteLine($"- {book.Title}: {daysOverdue} days overdue, Fine: ${fine:F2}");
                }
            }
        }

        System.Console.WriteLine("\nPress any key to continue...");
        System.Console.ReadKey();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Models
{
    public class Member
    {
        public const int MaxBorrowedBooks = 5;
        public const int DefaultLoanDays = 14;
        
        public string Name { get; }
        public string Id { get; }
        private readonly List<Book> _borrowedBooks;
        public IReadOnlyList<Book> BorrowedBooks => _borrowedBooks.AsReadOnly();

        public Member(string name, string id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be empty", nameof(id));
            
            if (!IsValidMemberId(id))
                throw new ArgumentException("Invalid member ID format", nameof(id));

            Name = name;
            Id = id;
            _borrowedBooks = new List<Book>();
        }

        public bool CanBorrow() => _borrowedBooks.Count < MaxBorrowedBooks;

        public void BorrowBook(Book book, DateTime? dueDate = null)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (!CanBorrow())
                throw new InvalidOperationException($"Member has reached the maximum limit of {MaxBorrowedBooks} borrowed books");

            if (_borrowedBooks.Any(b => b.ISBN == book.ISBN))
                throw new InvalidOperationException("Member has already borrowed this book");

            if (!book.IsAvailable())
                throw new InvalidOperationException("Book is not available for borrowing");

            var effectiveDueDate = dueDate ?? DateTime.Now.AddDays(DefaultLoanDays);
            book.MarkAsBorrowed(effectiveDueDate);
            _borrowedBooks.Add(book);
        }

        public void ReturnBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            var borrowedBook = _borrowedBooks.FirstOrDefault(b => b.ISBN == book.ISBN);
            if (borrowedBook == null)
                throw new InvalidOperationException("Member has not borrowed this book");

            borrowedBook.MarkAsReturned();
            _borrowedBooks.Remove(borrowedBook);
        }

        public List<Book> GetOverdueBooks(DateTime currentDate)
        {
            return _borrowedBooks
                .Where(book => book.IsOverdue(currentDate))
                .ToList();
        }

        private static bool IsValidMemberId(string id)
        {
            // Member ID format: 2 letters followed by 4 digits (e.g., AB1234)
            return System.Text.RegularExpressions.Regex.IsMatch(id, @"^[A-Z]{2}\d{4}$");
        }
    }
}

using System;
using LibrarySystem.Models.Enums;

namespace LibrarySystem.Models
{
    public class Book
    {
        public string Title { get; }
        public string Author { get; }
        public string ISBN { get; }
        public BookStatus Status { get; private set; }
        public DateTime? DueDate { get; private set; }

        public Book(string title, string author, string isbn)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty", nameof(author));

            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN cannot be empty", nameof(isbn));

            if (!IsValidISBNFormat(isbn))
                throw new ArgumentException("Invalid ISBN format", nameof(isbn));

            Title = title.Trim();
            Author = author.Trim();
            ISBN = isbn.Trim();
            Status = BookStatus.Available;
        }

        public bool IsAvailable() => Status == BookStatus.Available;

        public void MarkAsBorrowed(DateTime dueDate)
        {
            if (!IsAvailable())
                throw new InvalidOperationException("Book is not available for borrowing");

            Status = BookStatus.Borrowed;
            DueDate = dueDate;
        }

        public void MarkAsReturned()
        {
            if (Status != BookStatus.Borrowed)
                throw new InvalidOperationException("Book is not currently borrowed");

            Status = BookStatus.Available;
            DueDate = null;
        }

        public bool IsOverdue(DateTime currentDate)
        {
            return Status == BookStatus.Borrowed &&
                   DueDate.HasValue &&
                   currentDate.Date > DueDate.Value.Date;
        }

        private static bool IsValidISBNFormat(string isbn)
        {
            isbn = isbn.Replace("-", "").Replace(" ", "");
            // ISBN-10 format: 9 digits followed by an 'X' or a digit
            if (isbn.Length != 10)
                return false;

            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
            }

            char lastChar = isbn[9];
            return char.IsDigit(lastChar) || lastChar == 'X';
        }
        
        // private static bool IsValidISBN10(string isbn)
        // {
        //     isbn = isbn.Replace("-", "").Replace(" ", "");
        //     // length must be 10
        //     int n = isbn.Length;
        //     if (n != 10)
        //         return false;

        //     // Computing weighted sum of 
        //     // first 9 digits
        //     int sum = 0;
        //     for (int i = 0; i < 9; i++)
        //     {
        //         int digit = isbn[i] - '0';

        //         if (0 > digit || 9 < digit)
        //             return false;

        //         sum += (digit * (10 - i));
        //     }

        //     // Checking last digit.
        //     char last = isbn[9];
        //     if (last != 'X' && (last < '0' 
        //                     || last > '9'))
        //         return false;

        //     // If last digit is 'X', add 10 
        //     // to sum, else add its value.
        //     sum += ((last == 'X') ? 10 :
        //                     (last - '0'));

        //     // Return true if weighted sum 
        //     // of digits is divisible by 11.
        //     return (sum % 11 == 0);
        // }
    }
}

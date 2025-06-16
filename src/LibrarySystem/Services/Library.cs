using System;
using System.Collections.Generic;
using System.Linq;
using LibrarySystem.Common;
using LibrarySystem.Models;

namespace LibrarySystem.Services
{
    public class Library
    {
        private readonly List<Book> _books;
        private readonly List<Member> _members;

        public Library()
        {
            _books = new List<Book>();
            _members = new List<Member>();
        }

        public Result AddBook(Book book)
        {
            if (book == null)
                return Result.Failure("Book cannot be null");

            if (_books.Any(b => b.ISBN == book.ISBN))
                return Result.Failure("A book with this ISBN already exists");

            _books.Add(book);
            return Result.Success("Book added successfully", book);
        }

        public Result RegisterMember(Member member)
        {
            if (member == null)
                return Result.Failure("Member cannot be null");

            if (_members.Any(m => m.Id == member.Id))
                return Result.Failure("A member with this ID already exists");

            _members.Add(member);
            return Result.Success("Member registered successfully", member);
        }

        public Result BorrowBook(string memberId, string isbn)
        {
            var member = GetMemberById(memberId);
            if (member == null)
                return Result.Failure("Member not found");

            var book = _books.FirstOrDefault(b => b.ISBN == isbn);
            if (book == null)
                return Result.Failure("Book not found");

            try
            {
                member.BorrowBook(book);
                return Result.Success("Book borrowed successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Result ReturnBook(string memberId, string isbn)
        {
            var member = GetMemberById(memberId);
            if (member == null)
                return Result.Failure("Member not found");

            var book = _books.FirstOrDefault(b => b.ISBN == isbn);
            if (book == null)
                return Result.Failure("Book not found");

            try
            {
                member.ReturnBook(book);
                return Result.Success("Book returned successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public List<Book> SearchBooks(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Book>();

            query = query.ToLower();
            return _books.Where(book =>
                book.Title.ToLower().Contains(query) ||
                book.Author.ToLower().Contains(query) ||
                book.ISBN.Replace("-", "").Contains(query)
            ).ToList();
        }

        public List<Member> GetMembersWithOverdueBooks(DateTime currentDate)
        {
            return _members.Where(member => member.GetOverdueBooks(currentDate).Any()).ToList();
        }

        public List<Book> GetAllAvailableBooks()
        {
            return _books.Where(book => book.IsAvailable()).ToList();
        }

        public Member? GetMemberById(string memberId)
        {
            return _members.FirstOrDefault(m => m.Id == memberId);
        }

        public Book? GetBook(string isbn)
        {
            return _books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public int GetTotalBooks() => _books.Count;
        public int GetAvailableBooks() => _books.Count(b => b.IsAvailable());
        public int GetBorrowedBooks() => _books.Count(b => !b.IsAvailable());
        public int GetTotalMembers() => _members.Count;
    }
}

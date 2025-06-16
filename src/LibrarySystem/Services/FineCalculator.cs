using System;
using LibrarySystem.Models;

namespace LibrarySystem.Services
{
    public class FineCalculator
    {
        private readonly decimal _baseRate;
        private readonly decimal _maxFinePerBook;

        public FineCalculator(decimal baseRate = 0.50m, decimal maxFinePerBook = 30.00m)
        {
            if (baseRate <= 0)
                throw new ArgumentException("Base rate must be greater than zero", nameof(baseRate));
            
            if (maxFinePerBook <= 0)
                throw new ArgumentException("Maximum fine must be greater than zero", nameof(maxFinePerBook));

            _baseRate = baseRate;
            _maxFinePerBook = maxFinePerBook;
        }

        public decimal CalculateFine(Book book, DateTime currentDate)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (!book.DueDate.HasValue || currentDate <= book.DueDate.Value)
                return 0;

            var daysOverdue = (int)(currentDate - book.DueDate.Value).TotalDays;
            decimal totalFine = 0;

            // First 7 days at base rate ($0.50)
            if (daysOverdue > 0)
            {
                var daysAtBaseRate = Math.Min(7, daysOverdue);
                totalFine += daysAtBaseRate * _baseRate;
                daysOverdue -= daysAtBaseRate;
            }

            // Next 7 days at 1.5x base rate ($0.75)
            if (daysOverdue > 0)
            {
                var daysAtMidRate = Math.Min(7, daysOverdue);
                totalFine += daysAtMidRate * (_baseRate * 1.5m);
                daysOverdue -= daysAtMidRate;
            }

            // Remaining days at 2x base rate ($1.00)
            if (daysOverdue > 0)
            {
                totalFine += daysOverdue * (_baseRate * 2.0m);
            }

            return Math.Min(totalFine, _maxFinePerBook);
        }

        public decimal CalculateTotalFines(Member member, DateTime currentDate)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            var overdueBooks = member.GetOverdueBooks(currentDate);
            var totalFines = 0m;

            foreach (var book in overdueBooks)
            {
                totalFines += CalculateFine(book, currentDate);
            }

            return totalFines;
        }

        public (int DaysOverdue, decimal FineAmount) GetFineDetails(Book book, DateTime currentDate)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (!book.DueDate.HasValue || currentDate <= book.DueDate.Value)
                return (0, 0m);

            var daysOverdue = (int)(currentDate - book.DueDate.Value).TotalDays;
            var fineAmount = CalculateFine(book, currentDate);

            return (daysOverdue, fineAmount);
        }
    }
}

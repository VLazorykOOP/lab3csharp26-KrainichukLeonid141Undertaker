using static System.Console;
namespace Lab_3
{
    internal class Program
    {
        class Date : IComparable<Date>
        {
            protected int day;
            protected int month;
            protected int year;

            // Масив для назв місяців
            private static readonly string[] MonthNames =
            {
            "січня", "лютого", "березня", "квітня", "травня", "червня",
            "липня", "серпня", "вересня", "жовтня", "листопада", "грудня"
        };
            public Date(int d, int m, int y)
            {
                if (!IsValidDate(d, m, y))
                    WriteLine($"Попередження: Дата {d}.{m}.{y} некоректна!");

                this.day = d;
                this.month = m;
                this.year = y;
            }
            public int Day
            {
                get => day;
                set { if (IsValidDate(value, month, year)) day = value; }
            }

            public int Month
            {
                get => month;
                set { if (IsValidDate(day, value, year)) month = value; }
            }

            public int Year
            {
                get => year;
                set { if (IsValidDate(day, month, value)) year = value; }
            }
            public int Century => (year - 1) / 100 + 1;

            public bool IsValid() => IsValidDate(day, month, year);

            public static bool IsValidDate(int d, int m, int y)
            {
                if (y < 1 || m < 1 || m > 12 || d < 1) return false;
                int[] daysInMonth = { 31, (IsLeapYear(y) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                return d <= daysInMonth[m - 1];
            }

            private static bool IsLeapYear(int y) => (y % 4 == 0 && y % 100 != 0) || (y % 400 == 0);
            public void PrintLong() => WriteLine($"{day} {MonthNames[month - 1]} {year} року");
            public void PrintShort() => WriteLine($"{day:D2}.{month:D2}.{year}");

            private long TotalDays()
            {
                long total = day;
                for (int y = 1; y < year; y++) total += IsLeapYear(y) ? 366 : 365;
                for (int m = 1; m < month; m++)
                {
                    int[] daysInMonth = { 31, (IsLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                    total += daysInMonth[m - 1];
                }
                return total;
            }
            public static long DaysBetween(Date d1, Date d2) => Math.Abs(d1.TotalDays() - d2.TotalDays());

            public int CompareTo(Date other)
            {
                if (this.year != other.year) return this.year.CompareTo(other.year);
                if (this.month != other.month) return this.month.CompareTo(other.month);
                return this.day.CompareTo(other.day);
            }
        }
        abstract class Document
        {
            public string Number { get; set; }
            public double Amount { get; set; }

            public Document(string number, double amount)
            {
                Number = number;
                Amount = amount;
            }

            public abstract void Show();
        }

        class Bill : Document
        {
            public string Customer { get; set; }
            public Bill(string num, double sum, string customer) : base(num, sum) => Customer = customer;

            public override void Show() =>
                WriteLine($"[Рахунок] №{Number}, Клієнт: {Customer}, Сума: {Amount} грн");
        }

        class Invoice : Document
        {
            public string Goods { get; set; }
            public Invoice(string num, double sum, string goods) : base(num, sum) => Goods = goods;

            public override void Show() =>
                WriteLine($"[Накладна] №{Number}, Товар: {Goods}, Сума: {Amount} грн");
        }

        class Receipt : Document
        {
            public string Service { get; set; }
            public Receipt(string num, double sum, string service) : base(num, sum) => Service = service;

            public override void Show() =>
                WriteLine($"[Квитанція] №{Number}, Послуга: {Service}, Сума: {Amount} грн");
        }

        static void Main()
                {
                OutputEncoding = System.Text.Encoding.UTF8;

                Date[] dates = {
                new Date(10, 5, 2023),
                new Date(31, 2, 2022), // Некоректна
                new Date(1, 1, 2020),
                new Date(15, 12, 2025)
                };

                WriteLine("\nПеревірка коректності та вивід:");
                var validDates = new List<Date>();
                foreach (var d in dates)
                {
                    if (d.IsValid())
                    {
                        d.PrintShort();
                        validDates.Add(d);
                    }
                    else WriteLine("Знайдено некоректну дату!");
                }

                validDates.Sort();
                WriteLine("\nВпорядковані дати:");
                validDates.ForEach(d => d.PrintLong());

                long maxDiff = 0;
                for (int i = 0; i < validDates.Count; i++)
                    for (int j = i + 1; j < validDates.Count; j++)
                        maxDiff = Math.Max(maxDiff, Date.DaysBetween(validDates[i], validDates[j]));

                WriteLine($"\nНайбільша кількість днів між датами: {maxDiff}");

                Task2.Execute();
        }
        class Task2
        {
            public static void Execute()
            {
                WriteLine("\n--- Завдання 2 (Ієрархія документів) ---");

                Document[] docs = new Document[]
                {
                new Bill("A-101", 5400.50, "ТОВ 'Вектор'"),
                new Invoice("INV-55", 1200.00, "Офісний папір"),
                new Receipt("R-007", 350.00, "Доставка")
                };
                var sortedDocs = docs.OrderBy(d => d.Amount).ToArray();

                foreach (var d in sortedDocs)
                {
                    d.Show();
                }
            }
        }
    }
}

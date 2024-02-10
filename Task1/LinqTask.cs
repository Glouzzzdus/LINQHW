using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(c =>
            (customer: c,
            suppliers: suppliers.Where(s => string.Equals(s.City, c.City, StringComparison.OrdinalIgnoreCase))));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.GroupJoin(suppliers,
            customer => customer.City,
            supplier => supplier.City,
            (customer, supplierGroup) => (customer, supplierGroup));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers.Where(c => c.Orders.Any(o => o.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)))
                .OrderBy(c => c.dateOfEntry.Year)
                .ThenBy(c => c.dateOfEntry.Month)
                .ThenByDescending(c => c.customer.Orders.Sum(o => o.Total))
                .ThenBy(c => c.customer.CompanyName);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)))
                .OrderBy(c => c.dateOfEntry.Year)
                .ThenBy(c => c.dateOfEntry.Month)
                .ThenByDescending(c => c.customer.Orders.Sum(o => o.Total))
                .ThenBy(c => c.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers.Where(c =>
                c.PostalCode != null && c.PostalCode.Any(ch => !char.IsDigit(ch))
                || string.IsNullOrEmpty(c.Region)
                || c.Phone != null && !c.Phone.Contains("("));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            return products
                .GroupBy(p => p.Category)
                .Select(catGroup => new Linq7CategoryGroup
                {
                    Category = catGroup.Key,
                    UnitsInStockGroup = catGroup
                        .GroupBy(p => p.UnitsInStock)
                        .Select(stockGroup => new Linq7UnitsInStockGroup
                        {
                            UnitsInStock = stockGroup.Key,
                            Prices = stockGroup.OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice)
                        })
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(IEnumerable<Product> products, decimal cheap, decimal middle, decimal expensive)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            return new[]
            {
                (cheap, products.Where(p => p.UnitPrice <= cheap)),
                (middle, products.Where(p => p.UnitPrice > cheap && p.UnitPrice <= middle)),
                (expensive, products.Where(p => p.UnitPrice > middle && p.UnitPrice <= expensive))
             };
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));

            return customers
                .GroupBy(c => c.City)
                .Select(group => (
                    city: group.Key,
                    averageIncome: (int)Math.Round(group
                         .Where(c => c.Orders != null)
                         .Select(c => c.Orders.Sum(o => o.Total))
                         .Average()),
                    averageIntensity: (int)Math.Round(group
                         .Where(c => c.Orders != null)
                         .Average(c => c.Orders.Length))))
                .ToList();
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            if (suppliers == null)
                throw new ArgumentNullException(nameof(suppliers));

            return string.Concat(suppliers
                .Select(s => s.Country)
                .Distinct()
                .OrderBy(s => s.Length)
                .ThenBy(s => s));
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;
using NomOrderManager.Domain;

namespace NomOrderManager.Model
{
    public class OverviewModel : IOrdersModel
    {
        public OverviewModel(string serviceName, string phoneNumber, IEnumerable<Order> orders)
        {
            var tmp = orders.ToList();
            Name = $"{serviceName} - Zusammenfassung";
            ServiceName = serviceName;
            PhoneNumber = phoneNumber;
            GroupedOrders = tmp
                .GroupBy(o => new { o.Item, o.Comment })
                .Select(g => new GroupedOrder()
                {
                    Amount = g.Count(),
                    Name = g.Key.Item.Name,
                    Price = g.Key.Item.Price,
                    PriceSum = g.Key.Item.Price * g.Count(),
                    Comment = g.Key.Comment
                });

            foreach (var order in tmp)
            {
                if (order.HasComment)
                {
                    HasComment = true;
                    break;
                }
            }

            Count = tmp.Count();
            CountText = Count > 1 ? $"{Count} Bestellungen" : $"{Count} Bestellung";
            Total = GroupedOrders.Sum(g => g.PriceSum);
        }

        public string Name { get; }

        public string ServiceName { get; }

        public string PhoneNumber { get; }

        public IEnumerable<GroupedOrder> GroupedOrders { get; }

        public bool HasData { get { return GroupedOrders.Any(); } }

        public int Count { get; }

        public string CountText { get; }

        public decimal Total { get; }

        public string TotalText
        {
            get
            {
                return Total.ToString(Item.PriceFormatString);
            }
        }

        public bool HasComment { get; }
    }
}

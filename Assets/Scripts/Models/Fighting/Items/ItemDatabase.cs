using System;
using System.Collections.Generic;

namespace Models.Fighting.Items {
    public class ItemDatabase : IItemDatabase {
        public static readonly IItemDatabase Instance = new ItemDatabase();

        private readonly Dictionary<string, IItem> _items = new Dictionary<string, IItem>();

        public ItemDatabase() {
            Add(new Laudanum());
        }

        private void Add(IItem item) {
            if (_items.ContainsKey(item.Id)) {
                throw new ArgumentException("An item with id " + item.Id + " already exists.");
            }

            _items[item.Id] = item;
        }

        public IItem GetItemById(string id) {
            if (!_items.ContainsKey(id)) {
                throw new ArgumentException("No item registered by id " + id);
            }

            return _items[id];
        }
    }
}
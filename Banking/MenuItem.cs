using System;

namespace SimpleBankingSystem_CSharp
{
    internal class MenuItem
    {
        internal string Id { get; }
        private readonly string _description;
        internal Action Function { get; }

        internal MenuItem(string id, string name, Action function) {
            Id = id;
            _description = name;
            Function = function;
        }

        public override string ToString() {
            return Id + ". " + _description;
        }
    }
}
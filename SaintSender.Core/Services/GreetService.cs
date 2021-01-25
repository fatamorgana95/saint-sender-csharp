using System;
using SaintSender.Core.Interfaces;

namespace SaintSender.Core.Services
{
    public class GreetService : IGreetService
    {
        public string Greet(string name)
        {
            return $"Welcome {name}, my friend!";
        }
    }
}

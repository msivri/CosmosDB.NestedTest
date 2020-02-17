using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDB.NestedTest
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    }

    public class Library
    {
        public Guid Id { get; private set; }
        public string OwnerId { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();
        private List<Book> _books = new List<Book>();

        public Library() { }
        public Library(Guid id, Guid ownerId, List<Book> books)
        {
            Id = id;
            OwnerId = ownerId.ToString();
            _books = books;
        }
    }

    public class Book : Entity
    { 
        public string Name { get; private set; }
        public IReadOnlyCollection<Page> Pages => _pages.AsReadOnly();
        private List<Page> _pages = new List<Page>();

        public Book() { }
        public Book(string name, List<Page> pages)
        {
            Name = name;
            _pages = pages;
        }
    }

    public class Page : Entity
    { 
        public Page() { }
        public Page(int pageNumber, List<Note> notes, List<Highlight> highlights)
        {
            PageNumber = pageNumber;
            _notes = notes;
            _highlights = highlights;
        }

        public int PageNumber { get; private set; }
        public IReadOnlyCollection<Note> Notes => _notes.AsReadOnly();
        private List<Note> _notes = new List<Note>();
        public IReadOnlyCollection<Highlight> Highlights => _highlights.AsReadOnly();
        private List<Highlight> _highlights = new List<Highlight>();
    }
    public class Note : Entity
    {
        public Note() { }
        public Note(string notes)
        {
            Notes = notes;
        }

        public string Notes { get; private set; }
    }
    public class Highlight : Entity
    {
        public Highlight() { }
        public Highlight(string highlights)
        {
            Highlights = highlights;
        }

        public string Highlights { get; private set; }
    }
}

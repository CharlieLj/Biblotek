using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Skapa en instans av biblioteket
        Library library = new Library();

        // Oändlig loop som låter användaren interagera med bibliotekssystemet genom att välja olika alternativ.
        // Menyn visas för användaren, och utför olika åtgärder beroende på användarens val.
        while (true)
        {
            Console.WriteLine("Welcome to the library!");
            Console.WriteLine("1. Add new book");
            Console.WriteLine("2. Loan book");
            Console.WriteLine("3. Return book");
            Console.WriteLine("4. Show available books");
            Console.WriteLine("5. Show borrowers and their borrowed books");
            Console.WriteLine("0. Exit");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    library.AddNewBook();
                    break;
                case 2:
                    library.LoanBook();
                    break;
                case 3:
                    library.ReturnBook();
                    break;
                case 4:
                    library.ShowAvailableBooks();
                    break;
                case 5:
                    library.ShowBorrowersAndBooks();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}

// Olika klasser för book och borrower
// public: en åtkomstmodifierare som anger att egenskapen Title är tillgänglig från andra delar av programmet, inte bara inom samma klass.
// string: datatypen för egenskapen Title. 
// Title: namnet på egenskapen. 
// { get; set; }: är syntaxen för att skapa en automatisk egenskap för att hämta värdet för egenskapen. 
class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool Loaned { get; set; }
    public Borrower Borrower { get; set; }
}

class Borrower
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public List<Book> BorrowedBooks { get; set; } = new List<Book>();
}

// Library klassen hanterar böcker och låntagare.
class Library
{
    // Privata listor för att lagra information om böcker och låntagare
    private List<Book> books = new List<Book>();
    private List<Borrower> borrowers = new List<Borrower>();

    // Lägger till en ny bok i biblioteket genom att ta in titel och författare från användaren och skapa en ny instans av Bok-klassen.
    public void AddNewBook()
    {
        Console.Write("Enter book title: ");
        string title = Console.ReadLine();
        Console.Write("Enter author: ");
        string author = Console.ReadLine();

        // // Skapa en ny bok och lägg till den i listan med böcker
        Book newBook = new Book { Title = title, Author = author, Loaned = false };
        books.Add(newBook);

        Console.WriteLine("New book added.");
    }

    // Lånar ut en bok till en låntagare
    // Använder en foreach-loop för att söka igenom alla böcker i books-listan och hitta den bok som matchar den angivna titeln och inte är utlånad. Om boken hittas, utförs utlåningsåtgärden; annars meddelas användaren att antingen boken redan är utlånad eller inte finns i biblioteket.
    public void LoanBook()
    {
        Console.Write("Enter borrower's name: ");
        string name = Console.ReadLine();
        Console.Write("Enter borrower's phone number: ");
        string phoneNumber = Console.ReadLine();

        // Skapa en ny låntagare och lägg till den i listan över låntagare
        Borrower borrower = new Borrower { Name = name, PhoneNumber = phoneNumber };
        this.borrowers.Add(borrower);

        Console.Write("Enter book title to loan out: ");
        string title = Console.ReadLine();

        // Sök igenom böcker för att hitta den efterfrågade boken
        Book bookToLoan = null;

        foreach (var book in books)
        {
            if (book.Title == title && !book.Loaned)
            {
                bookToLoan = book;
                break; // boken hittad, bryter loopen
            }
        }

        // Utför utlåningsåtgärden om boken hittas
        if (bookToLoan != null)
        {
            bookToLoan.Loaned = true;
            bookToLoan.Borrower = borrower;
            borrower.BorrowedBooks.Add(bookToLoan);

            Console.WriteLine("Book loaned to " + borrower.Name + ".");
        }
        else
        {
            Console.WriteLine("The book is either already loaned or does not exist in the library.");
        }
    }

    // Återlämnar en bok som har varit utlånad
    // använder en foreach-loop för att söka igenom alla böcker i books-listan och hitta den bok som matchar det angivna titeln och som är utlånad. 
    public void ReturnBook()
    {
        Console.Write("Enter book title to return: ");
        string title = Console.ReadLine();

        // Sök igenom böcker för att hitta den efterfrågade boken
        Book bookToReturn = null;

        foreach (var book in books)
        {
            if (book.Title == title && book.Loaned)
            {
                bookToReturn = book;
                break; // hittat boken, bryt loopen
            }
        }

        // Kontrollera om boken att returnera har hittats
        if (bookToReturn != null)
        {
            // Återställ utlåningsstatus och kopplingar till låntagaren för den återlämnade boken
            bookToReturn.Loaned = false;
            bookToReturn.Borrower.BorrowedBooks.Remove(bookToReturn);
            bookToReturn.Borrower = null;

            Console.WriteLine("Book returned.");
        }
        else
        {
            Console.WriteLine("The book is either already returned or does not exist in the library.");
        }
    }

    // Använder en foreach-loop för att loopa igenom alla böcker i books-listan och lägga till dem i availableBooks-listan om de inte är utlånade.
    // Sedan används en annan foreach-loop för att skriva ut informationen om de tillgängliga böckerna. 
    public void ShowAvailableBooks()
    {
        List<Book> availableBooks = new List<Book>();

        // Loopa igenom varje bok i biblioteket
        foreach (var book in books)
        {
            // Kontrollera om boken inte är utlånad
            if (!book.Loaned)
            {
                // Lägg till den tillgängliga boken i en separat lista
                availableBooks.Add(book);
            }
        }

        // Skriv ut tillgängliga böcker
        Console.WriteLine("Available books:");

        // Loopa igenom varje tillgänglig bok och skriv ut dess information
        foreach (var book in availableBooks)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}");
        }
    }

    // Visar information om låntagare och de böcker de har lånat.
    public void ShowBorrowersAndBooks()
    {
        Console.WriteLine("Borrowers and their borrowed books:");

        // Loopa igenom varje låntagare i biblioteket
        foreach (var borrower in borrowers)
        {
            // Skriv ut information om låntagaren
            Console.WriteLine($"Borrower: {borrower.Name}, Phone Number: {borrower.PhoneNumber}");

            // Kontrollera om låntagaren har lånat några böcker
            if (borrower.BorrowedBooks.Count > 0)
            {
                Console.WriteLine("Borrowed books:");

                // Loopa igenom varje bok som låntagaren har lånat och skriv ut dess information
                foreach (var book in borrower.BorrowedBooks)
                {
                    Console.WriteLine($"  Title: {book.Title}, Author: {book.Author}");
                }
            }
            else
            {
                // Meddela användaren om låntagaren inte har några lånade böcker för tillfället
                Console.WriteLine("The borrower has no borrowed books at the moment.");
            }

            Console.WriteLine();
        }
    }
}

using NLog;
using System.Linq;
using Helper;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");


try
{
    char option;
    do {
        var db = new BloggingContext();
        Console.WriteLine("Enter your selection:");
        Console.WriteLine("1) Display all blogs");
        Console.WriteLine("2) Add Blog");
        Console.WriteLine("3) Create Post");
        Console.WriteLine("4) Display Posts");
        Console.WriteLine("Enter 'q' to quit");
        option = Inputs.GetChar("> ", new char[] {'1', '2', '3', '4', 'q'});
        switch(option) {

            case '1':
            // Display all Blogs from the database
            var query = db.Blogs.OrderBy(b => b.Name);

            Console.WriteLine("All blogs in the database:");
            foreach (var item in query)
            {
                Console.WriteLine(item.Name);
            }
            break;

            case '2':
            // Create and save a new Blog
            Console.Write("Enter a name for a new Blog: ");
            var name = Console.ReadLine();

            var blog = new Blog { Name = name };


            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
            break;

            case '3':
            // Create a blog post
            break;

            case '4':
            // Display blog posts
            break;
    }
    } while (option == '1' || option == '2' || option == '3' || option == '4');
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");

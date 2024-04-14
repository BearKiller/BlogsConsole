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
        var blogQuery = db.Blogs.OrderBy(b => b.Name);
        List<int> blogIDs = new List<int>();
        Console.WriteLine("\nEnter your selection:");
        Console.WriteLine("1) Display all blogs");
        Console.WriteLine("2) Add Blog");
        Console.WriteLine("3) Create Post");
        Console.WriteLine("4) Display Posts");
        Console.WriteLine("Enter 'q' to quit");
        option = Inputs.GetChar("> ", new char[] {'1', '2', '3', '4', 'q'});
        switch(option) {



            case '1':
            Console.Clear();
            logger.Info("User choice - 1) Display all blogs");
            // Display all Blogs from the database

            Console.WriteLine("All blogs in the database:");
            foreach (var item in blogQuery) {
                Console.WriteLine(item.BlogID + ") " + item.Name);
            } break;



            case '2':
            Console.Clear();
            logger.Info("User choice - 2) Add blog");
            // Create and save a new Blog
            Console.Write("Enter a name for a new Blog: ");
            var name = Console.ReadLine();

            var blog = new Blog { Name = name };

            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
            break;



            case '3':
            Console.Clear();
            logger.Info("User choice - 3) Create Post");
            // Create a blog post
            Console.Clear();
            Console.WriteLine("Select the blog you would like to post to: ");
            foreach (var item in blogQuery) {
                Console.WriteLine(item.BlogID + ") " + item.Name);
                blogIDs.Add(item.BlogID);
                }

            int blogChoice;
            bool blogChoiceSuccess = false;
            while (!blogChoiceSuccess) {
                blogChoice = Inputs.GetInt("> ");
                if (blogIDs.Contains(blogChoice)) {
                    blogChoiceSuccess = true;
                } else {
                    logger.Error("Input beyond range of blogIDs.");
                }
            }

            break;



            case '4':
            Console.Clear();
            logger.Info("User choice - 4) Display Posts");
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

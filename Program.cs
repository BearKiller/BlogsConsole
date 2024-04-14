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
        var blogQuery = db.Blogs.OrderBy(b => b.BlogID);
        var postQuery = db.Posts.OrderBy(p => p.PostId).ThenBy(p => p.Title);
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

            var blogsList = blogQuery.ToList();
            Console.WriteLine(blogsList.Count() + " blogs returned");
            foreach (var item in blogsList) {
                Console.WriteLine(" " + item.Name);
            } break;



            case '2':
            Console.Clear();
            logger.Info("User choice - 2) Add blog");
            // Create and save a new Blog
            var name = Inputs.GetString("Enter a name for a new Blog: ");

            var blog = new Blog { Name = name };

            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
            break;



            case '3':
            Console.Clear();
            logger.Info("User choice - 3) Create Post");
            blogIDs.Clear();
            // Create a blog post
            Console.Clear();
            Console.WriteLine("Select the blog you would like to post to: ");
            foreach (var item in blogQuery) {
                Console.WriteLine(item.BlogID + ") " + item.Name);
                blogIDs.Add(item.BlogID);
                }

            int blogChoice = 0;
            bool blogChoiceSuccess = false;
            while (!blogChoiceSuccess) {
                blogChoice = Inputs.GetInt("> ");
                if (blogIDs.Contains(blogChoice)) {
                    blogChoiceSuccess = true;
                } else {
                    logger.Error("Input beyond range of blogIDs.");
                }
            }

            var postName = Inputs.GetString("Enter a post title: ");
            var postContent = Inputs.GetString("Enter post content: ");
            var post = new Post {
                Title = postName,
                Content = postContent,
                BlogId = blogChoice,
            };
            db.AddPost(post);
            logger.Info("Post added - {postName}", postName);
            break; 



            case '4':
            Console.Clear();
            logger.Info("User choice - 4) Display Posts");
            blogIDs.Clear();
            // Display blog posts
            Console.WriteLine("Select the blog's posts to display: ");
            Console.WriteLine("0) Posts from all blogs");
            foreach (var item in blogQuery) {
                Console.WriteLine(item.BlogID + ") Posts from " + item.Name);
                blogIDs.Add(item.BlogID);
                }

            int postChoice = -1;
            bool postChoiceSuccess = false;
            while (!postChoiceSuccess) {
                postChoice = Inputs.GetInt("> ");
                if (blogIDs.Contains(postChoice) || postChoice == 0) {
                    postChoiceSuccess = true;
                } else {
                    logger.Error("Input beyond range of blogIDs.");
                }
            }

            if (postChoice == 0) {
                var blogs = blogQuery.ToList();
                var posts = postQuery.ToList();
                Console.WriteLine(posts.Count() + " posts returned.");
                foreach (var item in postQuery) {
                    var blogNameResult = blogs.FirstOrDefault(b => b.BlogID == item.BlogId)?.Name;
                    Console.WriteLine("Blog: " + blogNameResult);
                    Console.WriteLine("Title: " + item.Title);
                    Console.WriteLine("Content: " + item.Content);
                } 
            } else {
                var blogs = blogQuery.ToList();
                var posts = postQuery.ToList();
                var blogQueryResult = blogs.Where(b => b.BlogID == postChoice).FirstOrDefault().Name;
                var postQueryResult = posts.Where(p => p.BlogId == postChoice).ToList();
                Console.WriteLine(postQueryResult.Count() + " posts returned.");
                Console.WriteLine("Blog: " + blogQueryResult);
                foreach (var item in postQueryResult) {
                    Console.WriteLine("Title: " + item.Title);
                    Console.WriteLine("Content: " + item.Content);
                }
            }

            break;

        }
    } while (option == '1' || option == '2' || option == '3' || option == '4');
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");

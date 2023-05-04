using TaskManager.Models;

namespace TaskManager.Data;

public class AddDbInitializer
{
    //TODO add initializer

   /* public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using(var serviceScope =applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDBContext>();

            context.Database.EnsureCreated();

            //Category
            if(!context.Categories.Any())
            {
                new Category()
                {
                    CategoryName = "Manager"
                };
            }
            context.SaveChanges();
            //Status
            if (!context.Statuses.Any())
            {

            }
            //User
            if (!context.Users.Any())
            {

            }
            //Task
            if (!context.Tasks.Any())
            {

            }
            //Workspace
            if (!context.Workspaces.Any())
            {

            }
            //Workspace_User
            if (!context.Workspaces_Users.Any())
            {

            }
        }
    }*/
}

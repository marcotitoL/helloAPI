# helloAPI

I just recently started learning about dotnet and C# and i just stackoverflowed my way into learning how to get a working backend api up and running. 
The codes i wrote here probably could use a refactor or two and I hope you have a good sense of humour before scrutinizing them. My goal here is to
learn the essentials and get something up and running and have some self-fulfillment(?) in the process. 

### Things I learned while doing this
* Connect to a MySQL database
* Seeding data (*although kind of a hacky way*)
* Using dotnet's builtin login/signin
* Using dotnet's Aspnet Users table as base users table and just referencing it from additional tables
* Utilizing JWT login tokens
* Authentication and Authorization *(a little bit on Roles)*
* LINQ - *mostly methods but i tried to use the query syntax*
* Records and DTOs (*i'm not sure if used them the **RIGHT** way tbh*)
* Attributes on Property, Class, Methods
* handling file uploads and using static files
* Customizing swagger
* How to deploy dotnet apps on Ubuntu using nginx *(got a lot of some headscratchers here)*

you can see all of this in action here -> https://helloapi.codewithmar.co/swagger/index.html

___________

#### want to run this locally?
1. Make sure you have all the required dependencies installed to run the latest dotnet core on your machine
2. clone this REPO and cd to the project directory

      `git clone https://github.com/marcotitoL/helloAPI.git`
3. rename sample.appsettings.json to appsettings.json and update the connection string
     
     ![screenshot](https://prnt.sc/6M14GkM9-FMM)
     
4. run `dotnet ef database update`
      
      *NOTE: if you haven't installed the entity framework tool you need to install it by running `dotnet tool install --global dotnet-ef` before running the database update*

5. run `dotnet run`

_____________

_**note** that i'm updating this repo constantly as i still have a LOT to learn, i have a lot of TODOs in my mind ATM. Next im gonna do is to put some detailed comment
on the siginificant lines of code_

# EcoStore
A simple online store for eco-products built using ASP .NET MVC and EF Core


## Features
- #### For clients
  - Browse all of the products
  - Filter products based on: their name, price, category, brand.
  - See details about each category and brand
  - Sign up
  - Sign in
  - Change profile info, including password
  - Place orders
  - View your orders
- #### For admins
  - Manage all of the products, categories and brands
  - Manage users and orders
  - Register new admin accounts
  - Generate reports (sales reports, products reports and order reports)


## Prerequisites
- .NET 7 SDK
- SQL Server 2022
- dotnet-ef tool


## How to run
#### 1. Clone this git repository
```
git clone https://github.com/Cubball/EcoStore.git
```

#### 2. Go into the EcoStore folder
```
cd EcoStore
```

#### 3. Set all of the necessary configuration values
They include:
- ConnectionStrings:DefaultConnection - a default connection string
- Admin:Email - an email that will be used to create the initial admin account
- Admin:Password - password for that account
- Stripe:PublishableKey - a publishable key for Stripe
- Stripe:SecretKey - a secret key for Stripe

You can put these values in the src/EcoStore.Presentation/appsettings.json file. Example:
```json
"ConnectionStrings": {
      "DefaultConnection": "your_connection_string"
}
```
Or you can use .NET User Secrets feature. For that, initialize it:
```
dotnet user-secrets init --project src/EcoStore.Presentation
```
Then, set each of the values using the following command:
```
dotnet user-secrets set "key" "value" --project src/EcoStore.Presentation
```

#### 4. Apply migrations
After you've provided your own SQL Server connection string you can apply migrations:
```
dotnet ef database update --project src/EcoStore.DAL --startup-project src/EcoStore.Presentation
```

#### 5. Run the app
```
dotnet run --project src/EcoStore.Presentation
```
